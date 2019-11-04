//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SpawnDoorTrigger : MonoBehaviour
//{
//    private SpawnDoor spawnDoor;

//    // Start is called before the first frame update
//    void Start()
//    {
//        spawnDoor = FindObjectOfType<SpawnDoor>();
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.tag == "Player")
//        {
//            spawnDoor.playerTriggeredDoor = true;
//            Destroy(gameObject);
//        }
//    }
//}
