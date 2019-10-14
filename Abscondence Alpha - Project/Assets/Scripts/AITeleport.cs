using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITeleport : MonoBehaviour
{
    public float maxTetherDistance;
    private NavMeshAgent aiFollower;
    private PlayerController player;
    private TeleportPlayer teleport;

    // Start is called before the first frame update
    void Start()
    {
        aiFollower = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        teleport = GameObject.Find("Player_Sender").GetComponent<TeleportPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerPosition = player.transform.position;
        var aiPosition = transform.position;

        aiFollower.SetDestination(playerPosition);

        if (teleport.playerTeleported)
        {
            AgentReposition();
            teleport.playerTeleported = false;
        }

        if (Vector3.Distance(player.transform.position, transform.position) > maxTetherDistance)
        {
            transform.position = playerPosition;
        }


    }

    void AgentReposition()
    {
        //disable everything agent related
        aiFollower.isStopped = true;
        aiFollower.updatePosition = false;
        aiFollower.updateRotation = false;
        //this is the most important part; turn the agent off:
        aiFollower.enabled = false;

        // Teleport the agent to
        transform.position = player.transform.position;

        //restore everything
        aiFollower.enabled = true;
        aiFollower.updatePosition = true;
        aiFollower.updateRotation = true;
    }
}
