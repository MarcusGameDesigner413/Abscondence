using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable_Block : MonoBehaviour
{
    private PlayerController player;
    private Vector3 relativePosition;
    private Vector3 playerPosition;
    private Vector3 boxPosition;
    private Transform tempTrans;
    private bool playerDragging = false;
    private Vector3 playerVelocity;
    private Vector3 velocity;
    private Rigidbody box;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        velocity = GetComponent<Rigidbody>().velocity;
        //GetComponent<Rigidbody>().isKinematic = true;
        box = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = player.transform.position;
        boxPosition = transform.position;

        relativePosition = boxPosition - playerPosition;

        playerVelocity = player.draggableBlockVelocity; /*player.GetComponent<CharacterController>().velocity;*/

        //relativePosition.x = (boxPosition.x - playerPosition.x);
        //relativePosition.z = boxPosition.z - playerPosition.z;
        relativePosition.y = 0;

        velocity.y = 0;

            GetComponent<Rigidbody>().velocity = /*player.GetComponent<CharacterController>().velocity*/ playerVelocity;
        //Debug.Log(player.draggableBlockVelocity);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetButton("Interact"))
        {
            playerDragging = true;
            DraggingState(true);


            //playerDragging = !playerDragging;
        }
        else
            DraggingState(false);
    }

    void DraggingState(bool dragging)
    {
        if (dragging)
        {
            Debug.Log(playerVelocity);
           // box.AddForce(player.GetComponent<CharacterController>().velocity);
        }
    }
}