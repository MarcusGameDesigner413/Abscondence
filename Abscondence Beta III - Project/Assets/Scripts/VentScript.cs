using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentScript : MonoBehaviour
{
    public bool isLocked;
    public float speed;
    float startTime;
    float journeyLength;
    float distCovered;
    float fractionOfJourney;
    int index;
    public GameObject targetVent;
    public Camera gameCamera;
    public Camera miniMapCamera;
    public GameObject player;
    public Vector3 teleportOffset;
    public GameObject[] ventPoints;

    bool inVent;
    bool hasDestinationBeenSet;
    Vector3 cameraOffset;
    Vector3 miniMapCameraOffset;
    bool inMovement = false;

    // Update is called once per frame
    void Update()
    {
        if (inVent)
        {
            if (index + 1 == ventPoints.Length)
            {
                inVent = false;
<<<<<<< HEAD
=======
                //player.SetActive(true);
>>>>>>> master
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<CharacterController>().transform.SetPositionAndRotation(targetVent.transform.position + teleportOffset, player.transform.rotation);
                player.GetComponent<CharacterController>().enabled = true;
                player.gameObject.GetComponent<MeshRenderer>().enabled = true;
                player.gameObject.GetComponent<PlayerController>().enabled = true;
                gameCamera.gameObject.GetComponent<CameraMovement>().enabled = true;
                miniMapCamera.gameObject.GetComponent<MiniMapFollow>().enabled = true;
                if (targetVent.GetComponent<VentScript>().isLocked == true)
                {
                    targetVent.GetComponent<VentScript>().isLocked = false;
                }
            }
            else
            {
                if (!inMovement)
                {
                    journeyLength = Vector3.Distance(ventPoints[index].transform.position, ventPoints[index + 1].transform.position);
                    startTime = Time.time;
                    inMovement = true;
                }
                else
                {
                    distCovered = (Time.time - startTime) * speed;
                    fractionOfJourney = distCovered / journeyLength;
                    gameCamera.transform.position = (Vector3.Lerp(ventPoints[index].transform.position + cameraOffset, ventPoints[index + 1].transform.position + cameraOffset, fractionOfJourney));
                    miniMapCamera.transform.position = (Vector3.Lerp(ventPoints[index].transform.position + miniMapCameraOffset, ventPoints[index + 1].transform.position + miniMapCameraOffset, fractionOfJourney));
                }

                if (fractionOfJourney >= 1)
                {
                    index++;
                    inMovement = false;
                    fractionOfJourney = 0;
                    cameraOffset = gameCamera.transform.position - ventPoints[index].transform.position;
                    miniMapCameraOffset = miniMapCamera.transform.position - ventPoints[index].transform.position;
                    //miniMapCameraOffset.y = miniMapCamera.transform.position.y;
                }
            }
        }
        //If player interacts
        //If camera is stationary
        //Determine destination
        //Determine journey length
        //Set timer to zero
        //If camera is moving
        //Update distance covered
        //If camera has reached final destination
        //Set inVent to false
        //Teleport player
        //Store previous camera location
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (Input.GetButton("Interact") && !isLocked)
            {
                if(!inVent)
                {
                    index = 0;
                    inVent = true;
                    cameraOffset = gameCamera.transform.position - ventPoints[index].transform.position;
                    miniMapCameraOffset = miniMapCamera.transform.position - ventPoints[index].transform.position;
<<<<<<< HEAD
                   // miniMapCameraOffset.y = miniMapCamera.transform.position.y;
=======
                    //miniMapCameraOffset.y = miniMapCamera.transform.position.y;
                    //player.gameObject.SetActive(false);
>>>>>>> master
                    player.gameObject.GetComponent<MeshRenderer>().enabled = false;
                    player.gameObject.GetComponent<PlayerController>().enabled = false;
                    gameCamera.gameObject.GetComponent<CameraMovement>().enabled = false;
                    miniMapCamera.gameObject.GetComponent<MiniMapFollow>().enabled = false;
                }
            }
        }
    }
}