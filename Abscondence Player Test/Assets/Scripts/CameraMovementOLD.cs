using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementOLD : MonoBehaviour
{
    // Start is called before the first frame update 
    public GameObject player;
    public Camera mainCam;
    public int xDiscreprency = 0;
    public int yDiscreprency = 12;
    public int zDiscreprency = -3;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mainCam.transform.position = new Vector3 (player.transform.position.x + xDiscreprency, player.transform.position.y + yDiscreprency, player.transform.position.z + zDiscreprency);
    }
}
