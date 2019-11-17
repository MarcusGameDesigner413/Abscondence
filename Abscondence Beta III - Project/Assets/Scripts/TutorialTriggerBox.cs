using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTriggerBox : MonoBehaviour
{
    [HideInInspector]
    public bool playerEnteredTrigger = false;
    [HideInInspector]
    public bool playerExitedTrigger = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player Entered Trigger box");
            playerEnteredTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player Exited Trigger box");
            playerExitedTrigger = true;
        }
    }
}
