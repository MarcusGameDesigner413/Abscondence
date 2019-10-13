﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public GameObject teleportTarget;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 teleportDestination = teleportTarget.transform.position - other.transform.position;
        Debug.Log("God has entered the teleporter");
        if (other.tag == "Player")
        {
            Debug.Log("God has abandoned us");
            other.GetComponent<CharacterController>().enabled = false;
            other.GetComponent<CharacterController>().transform.SetPositionAndRotation(teleportTarget.transform.position, other.transform.rotation);
            other.GetComponent<CharacterController>().enabled = true;
             
            // CharacterController.Move is off a little bit
            //other.GetComponent<CharacterController>().Move(teleportDestination);
        }

    }

}
