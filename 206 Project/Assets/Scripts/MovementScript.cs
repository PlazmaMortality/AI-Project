using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    //Refrences to gameobjects and components
    public CharacterController controller;
    public Transform cam;
    public Light light;
    public Color myColor;

    //Movement variables
    public float speed = 10f;
    public float smoothTurnTime = 0.1f;
    float turnSmoothVelocity;

    //Checks fro other scripts
    public bool inStealth;
    public bool isMoving;

    //Whistle
    public bool whistle;

    //Stamina
    public GameObject stamina;
    public Stamina stamReference;

    void Start()
    {
        myColor = new Color(1.5f, 0.64f, 0f, 1f);
        light.color = myColor;
        whistle = false;
        stamReference = stamina.GetComponent<Stamina>();
    }

    //Running and Stealth input
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && (stamReference.currentStamina > 0.1))
        {
            speed = 15f;

            if (isMoving)
            {
                stamReference.Run(40 * Time.deltaTime);
            }
        }
        else if(stamReference.currentStamina < 0.1)
        {
            speed = 10f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 10f;
        }

        if (Input.GetKeyDown("c"))
        {
            light.intensity = 0.5f;
            speed = 5f;
            inStealth = true;
        }

        if (Input.GetKeyUp("c"))
        {
            light.intensity = 1f;
            speed = 10f;
            inStealth = false;
        }

        if (Input.GetKeyDown("f"))
        {
            whistle = true;
        }

        if (Input.GetKeyUp("f"))
        {
            whistle = false;
        }

        //Player movement and look
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTurnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection.normalized * speed * Time.deltaTime);
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
    }
}
