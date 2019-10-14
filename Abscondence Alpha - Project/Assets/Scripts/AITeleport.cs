using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AITeleport : MonoBehaviour
{
    private NavMeshAgent aiFollower;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        aiFollower = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerPosition = player.transform.position;
        var aiPosition = transform.position;

        aiFollower.SetDestination(playerPosition);

    }
}
