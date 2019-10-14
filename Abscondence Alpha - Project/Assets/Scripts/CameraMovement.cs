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
    #region Zoom Mode 1
    public float xZoom1 = 0;
    public float yZoom1 = 12;
    public float zZoom1 = -3;
    public float xRot1 = 75;
    public float yRot1 = 0;
    public float zRot1 = 0;
    #endregion
    #region Zoom Mode 2
    public float xZoom2 = 0;
    public float yZoom2 = 0;
    public float zZoom2 = 0;
    public float xRot2 = 0;
    public float yRot2 = 0;
    public float zRot2 = 0;
    #endregion
    #region Zoom Mode 3
    public float xZoom3 = 0;
    public float yZoom3 = 0;
    public float zZoom3 = 0;
    public float xRot3 = 0;
    public float yRot3 = 0;
    public float zRot3 = 0;
    #endregion
    public float maxCameraMovement = 5;
    [Range(1, 3)]
    public int cameraMode = 0;
    #endregion

	// Update is called once per frame
	void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (!firstPersonMode)
        {
            float v = Input.GetAxis("CameraVertical");
            float h = Input.GetAxis("CameraHorizontal");
            if (h != 0 || v != 0)
                player.GetComponent<PlayerController/*Change to whatever is making the player move, preferably script*/>().enabled = false;
            else
                player.GetComponent<PlayerController/*Change to whatever is making the player move, preferably script*/>().enabled = true;

            mainCam.transform.position = new Vector3(player.transform.position.x + (xDiscreprency + (h * maxCameraMovement)), 
                                                     player.transform.position.y + yDiscreprency, 
                                                     player.transform.position.z + (zDiscreprency + (v * maxCameraMovement)));

            mainCam.transform.rotation = Quaternion.Euler(xRot, yRot, zRot);

            if (Input.GetKeyDown("c"))
            {
                if(cameraMode == 1)
                {
                    xDiscreprency = xZoom1;
                    yDiscreprency = yZoom1;
                    zDiscreprency = zZoom1;
                    xRot = xRot1;
                    yRot = yRot1;
                    zRot = zRot1;
                    cameraMode = 2;
                }
                else if (cameraMode == 2)
                {
                    xDiscreprency = xZoom2;
                    yDiscreprency = yZoom2;
                    zDiscreprency = zZoom2;
                    xRot = xRot2;
                    yRot = yRot2;
                    zRot = zRot2;
                    cameraMode = 3;
                    Debug.Log(zDiscreprency);
                }
                else if (cameraMode == 3)
                {
                    xDiscreprency = xZoom3;
                    yDiscreprency = yZoom3;
                    zDiscreprency = zZoom3;
                    xRot = xRot3;
                    yRot = yRot3;
                    zRot = zRot3;
                    cameraMode = 1;
                }
            }

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
                mainCam.cullingMask ^= 1 << LayerMask.NameToLayer("MiniMap");
                firstPersonMode = false;
            }
        }
    }
}
