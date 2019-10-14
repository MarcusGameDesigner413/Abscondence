using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomlessPit : MonoBehaviour
{
    public float fallAmount;
    public AITeleport aiFollower;
    private Vector3 falling;
    private bool playerFallen = false;
    PlayerController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //aiFollower = GetComponent<AITeleport>();
    }

    void Update()
    {
        if (player.transform.position.y < -fallAmount)
        {
            Debug.Log("Player dead");
            TeleportToAI();
            playerFallen = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
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
