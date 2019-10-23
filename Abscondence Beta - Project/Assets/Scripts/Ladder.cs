using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ladder : MonoBehaviour
{
    public GameObject teleportTarget;

    [HideInInspector]
    public bool playerTeleported = false;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 teleportDestination = teleportTarget.transform.position - other.transform.position;
        if (other.tag == "Player")
        {
            playerTeleported = true;
            other.GetComponent<CharacterController>().enabled = false;
            other.GetComponent<CharacterController>().transform.SetPositionAndRotation(teleportTarget.transform.position, other.transform.rotation);
            other.GetComponent<CharacterController>().enabled = true;
        }
        else if (other.tag == "Enemy")
        {
            var enemyAi = other.gameObject.GetComponent<NavMeshAgent>();
            // Disable everything agent related
            enemyAi.isStopped = true;
            enemyAi.updatePosition = false;
            enemyAi.updateRotation = false;
            // This is the most important part; turn the agent off:
            enemyAi.enabled = false;

            // Teleport the agent to the player
            other.transform.position = teleportTarget.transform.position;

            // Restore everything
            enemyAi.enabled = true;
            enemyAi.updatePosition = true;
            enemyAi.updateRotation = true;
        }

    }

}
