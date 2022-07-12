using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCollision : MonoBehaviour
{
    //References to pauseMenu script
    public GameObject Menu;
    public PauseMenu keyReference;
    public GameObject needKeyInfo;

    private void Start()
    {
        keyReference = Menu.GetComponent<PauseMenu>();
    }

    //FUnction called when a collision occurs between objects, whether the key or exit door.
    void OnTriggerEnter(Collider other)
    {
        if(gameObject.name == "KeyShape")
        {
            KeyInteraction(other);
        }

        if(gameObject.name == "Door")
        {
            ExitInteraction(other);
        }
        
    }

    // Destroys the object and sets key collected to true when the player collides with the key.
    void KeyInteraction(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Destroy(gameObject);
            keyReference.keyCollected = true;
            FindObjectOfType<AudioManager>().Play("Achievement");
        }
    }

    // Sets game won to true when the player collides with the door, after they have obtained the key.
    void ExitInteraction(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if(keyReference.keyCollected == true)
            {
                keyReference.gameWon = true;
            }
            else
            {
                needKeyInfo.SetActive(true);
                StartCoroutine(fadeText());
            }
        }
    }

    //Removes the key text after 2 seconds
    IEnumerator fadeText()
    {
        yield return new WaitForSeconds(2);

        needKeyInfo.SetActive(false);
    }
}
