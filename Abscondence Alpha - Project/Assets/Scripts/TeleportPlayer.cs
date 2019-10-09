using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
	public GameObject teleportTarget;

	private void OnTriggerEnter(Collider other)
	{
        Debug.Log("God has entered the teleporter");
            if (other.tag == "Player")
            {
                Debug.Log("God has abandoned us");
                other.GetComponent<Rigidbody>().transform.SetPositionAndRotation(teleportTarget.transform.position, other.transform.rotation);
            }
      
	}

}
