using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//States
public enum States { Patrol, Seek, Investigate, Attack}

public class AIScript : MonoBehaviour
{
    //AI and Player references
    public NavMeshAgent agent;
    public Transform player;

    //Lighting
    public Light light;
    public Color myColor;

    //Layer masks
    public LayerMask environmentMask, playerMask;

    //Patrol variables
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Ranges
    public float sightRange, attackRange, audioRange;

    //Investigate variables
    public Vector3 lastKnownPosition;

    //Referencing Player script 
    public GameObject thePlayer;
    public MovementScript playerScript;

    //Current state of the AI
    States currentState;

    // Pause Menu Reference
    public PauseMenu pMenu;

    //Initialises states and references
    private void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        playerScript = thePlayer.GetComponent<MovementScript>();
        walkPointSet = false;
        currentState = States.Patrol;
    }

    //Updates state checks and AI actions
    private void Update()
    {
        InvestigationCheck();
        SeekCheck();
        AttackCheck();
        StateMachine();
    }

    //Searches for a point on the navigatio mesh to walk to, with a blue light signifying the Patrol state
    void Patrol()
    {
        myColor.r = 0;
        myColor.g = 0;
        myColor.b = 1;
        myColor.a = 1;
        light.color = myColor;
        agent.speed = 5;

        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1.5f)
        {
            walkPointSet = false;
        }
    }

    //Chases the player while in range after the player is seen or sensed, Red Light
    void Seek()
    {
        //FindObjectOfType<AudioManager>().Play("Seek");
        myColor.r = 1;
        myColor.g = 0;
        myColor.b = 0;
        myColor.a = 1;
        light.color = myColor;
        agent.SetDestination(player.position);
        agent.speed = 12;

        if (!Physics.CheckSphere(transform.position, sightRange, playerMask))
        {
            lastKnownPosition = player.position;
            currentState = States.Investigate;
        }
    }

    //Walks to the last place that the player was seen/sensed, and patrols the area before returning to patrol. Yellow Light
    void Investigate()
    {
        myColor.r = 1;
        myColor.g = 0.92f;
        myColor.b = 0.016f;
        myColor.a = 1;
        light.color = myColor;
        agent.speed = 5;

        agent.SetDestination(lastKnownPosition);

        Vector3 distanceToWalkPoint = transform.position - lastKnownPosition;

        if (distanceToWalkPoint.magnitude < 1.5f)
        {
            StartCoroutine(SurveyArea());
        }
    }

    //Waits 2 seconds at the last known position of the player, then returns to patrol
    private IEnumerator SurveyArea()
    {
        yield return new WaitForSeconds(2);

        currentState = States.Patrol;
    }

    //Attacks the player if in range, resulting in game over
    void Attack()
    {
        agent.SetDestination(transform.position);
        pMenu.gameLost = true;
    }

    //Searches for a random point in the x and z directions on the nav mesh, through the use of a raycast
    void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 5f, environmentMask))
        {
            walkPointSet = true;
        }
    }

    //Checks if a player can be heard or seen with the audio range, for investigation
    void InvestigationCheck()
    {
        if (currentState != States.Seek)
        {
            if (Physics.CheckSphere(transform.position, audioRange, playerMask))
            {
                if (!playerScript.inStealth && playerScript.isMoving)
                {
                    lastKnownPosition = player.position;
                    currentState = States.Investigate;
                }

                if (playerScript.whistle)
                {
                    lastKnownPosition = player.position;
                    currentState = States.Investigate;
                }
            }
        }
    }

    //Checks if the player is seen within the sight range, with the use of a raycast
    void SeekCheck()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 15;
        Vector3 altPosition = transform.position;

        altPosition.y = altPosition.y + 1;
        Debug.DrawRay(altPosition, forward, Color.green);

        if (!Physics.Raycast(altPosition, forward, sightRange, environmentMask))
        {
            if (Physics.Raycast(altPosition, forward, sightRange, playerMask))
            {
                currentState = States.Seek;
            }
        }
    }

    //Checks if the player is within the attack range
    void AttackCheck()
    {
        if (Physics.CheckSphere(transform.position, attackRange, playerMask))
        {
            currentState = States.Attack;
        }
    }

    //Finite State Machine for the various states (Patrol, Seek, Investigate, Attack)
    void StateMachine()
    {
        if (currentState == States.Patrol)
        {
            Patrol();
        }

        if (currentState == States.Seek)
        {
            Seek();
        }

        if (currentState == States.Investigate)
        {
            Investigate();
        }

        if (currentState == States.Attack)
        {
            Attack();
        }
    }

    //Shows the radious and distance of the various ranges within the Unity editor.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, audioRange);
    }
    
}
