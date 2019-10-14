using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomlessPit : MonoBehaviour
{
    public AITeleport aiFollower;
    private Vector3 falling;
    private float fallAmount;
    PlayerController player;

    [HideInInspector]
    public bool playerFallen = false;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        fallAmount = player.startingHeight + 5.0f;
    }

    void Update()
    {
        if (player.transform.position.y < -fallAmount)
        {
           // Debug.Log("Player fallen");
            TeleportToAI();
            playerFallen = false;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerFallen = true;
            Debug.Log("Player in pit");
        }
    }

    public void TeleportToAI()
    {
        CharacterController controller = player.GetComponent<CharacterController>();

       controller.enabled = false;
       controller.transform.SetPositionAndRotation(aiFollower.transform.position, aiFollower.transform.rotation);
       controller.enabled = true;
        //player.GetComponent<CharacterController>().Move(aiFollower.transform.position);
    }
}
