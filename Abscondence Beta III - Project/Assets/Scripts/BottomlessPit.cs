﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomlessPit : MonoBehaviour
{
    private AITeleport aiFollower;
    private Vector3 falling;
    private float fallAmount;
    PlayerController player;

    [HideInInspector]
    public bool playerFallen = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        aiFollower = GameObject.Find("AIFollow").GetComponent<AITeleport>();
        fallAmount = player.startingHeight + 5.0f;
    }

    void Update()
    {
        // If the player falls below a certain Y level, it will teleport to the AIFollower and take damage
        if (player.transform.position.y < -fallAmount)
        {
            Debug.Log("Player fallen off map");
            TeleportToAI();
            player.currentHealth -= 2; // Player takes damage upon falling into hole
            playerFallen = false;
        }
    }

    // Check if the player has fallen in the pit
    void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.tag == "Player")
        {
            playerFallen = true;
            Debug.Log("Player in pit");
        }

        //if you are an enemy
        if(other.gameObject.tag == "Enemy" && !other.gameObject.GetComponent<TrooperBehaviour>().deathByPit)
        {

            Debug.Log("THIS IS SPARTA!");

            //setting values to trigger the pit death
            other.gameObject.GetComponent<TrooperBehaviour>().deathByPit = true;
            other.gameObject.GetComponent<TrooperBehaviour>().xIsDeadX = true;

            //set the trooper position to the position of the bottomless pit
            other.transform.position = transform.position;
                
           
            
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