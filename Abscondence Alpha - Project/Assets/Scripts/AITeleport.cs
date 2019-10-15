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
        // Assingning variables for positions
        var playerPosition = player.transform.position;
        var aiPosition = transform.position;

        // Make the AI use the navmesh to follow the player
        aiFollower.SetDestination(playerPosition);

        if (teleport.playerTeleported)
        {
            AgentReposition();
            teleport.playerTeleported = false;
        }

        // If they player gets too far away from the AI, teleport the AI to the player
        if (Vector3.Distance(player.transform.position, transform.position) > maxTetherDistance)
        {
            transform.position = playerPosition;
        }


    }

    // If the player used the teleporter, reposition the AI to the player and move onto a new NavMesh
    void AgentReposition()
    {
        // Disable everything agent related
        aiFollower.isStopped = true;
        aiFollower.updatePosition = false;
        aiFollower.updateRotation = false;
        // This is the most important part; turn the agent off:
        aiFollower.enabled = false;

        // Teleport the agent to the player
        transform.position = player.transform.position;

        // Restore everything
        aiFollower.enabled = true;
        aiFollower.updatePosition = true;
        aiFollower.updateRotation = true;
    }
}
