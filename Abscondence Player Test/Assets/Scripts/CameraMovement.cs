using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public Camera mainCam;

    int everythingMask = -1;
    #region First Person Mode
    public bool firstPersonMode = false;
    public float firstPersonYDiscrepency = 1.0f;
    public float firstPersonHSpeed = 270.0f;
    public float firstPersonVSpeed = 270.0f;
    #endregion
    #region Third Person Mode
    public float xDiscreprency = 0;
    public float yDiscreprency = 12;
    public float zDiscreprency = -3;
    public float xRot = 75;
    public float yRot = 0;
    public float zRot = 0;
    public float maxCameraMovement = 5;
    #endregion

	// Update is called once per frame
	void Update()
    {

        if (!firstPersonMode)
        {
            float v = Input.GetAxis("CameraVertical");
            float h = Input.GetAxis("CameraHorizontal");
            if (h != 0 || v != 0)
                player.GetComponent<CharacterController/*Change to whatever is making the player move, preferably script*/>().enabled = false;
            else
                player.GetComponent<CharacterController/*Change to whatever is making the player move, preferably script*/>().enabled = true;

            mainCam.transform.position = new Vector3(player.transform.position.x + (xDiscreprency + (h * maxCameraMovement)), player.transform.position.y + yDiscreprency, player.transform.position.z + (zDiscreprency + (v * maxCameraMovement)));
            mainCam.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);

            if (Input.GetKeyDown("f"))
            {
                mainCam.cullingMask ^= 1 << LayerMask.NameToLayer("Player");
                firstPersonMode = true;
            }
        }
        else if (firstPersonMode)
        {
            //Look Horizontal - Turn the player left and right
            float fMouseX = Input.GetAxis("CameraHorizontal") * firstPersonHSpeed * Time.deltaTime;

            Vector3 playerRotation = transform.rotation.eulerAngles;
            playerRotation.y += fMouseX;

            transform.rotation = Quaternion.Euler(playerRotation);

            Vector3 cameraRotation = player.transform.rotation.eulerAngles;

            mainCam.transform.rotation = Quaternion.Euler(0, cameraRotation.y, cameraRotation.z);

            mainCam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + firstPersonYDiscrepency, player.transform.position.z);

            if (Input.GetKeyDown("f"))
            {
                mainCam.cullingMask = everythingMask;
                firstPersonMode = false;
            }
        }
    }
}
