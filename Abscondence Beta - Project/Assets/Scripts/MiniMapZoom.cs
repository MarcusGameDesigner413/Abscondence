using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapZoom : MonoBehaviour
{
    public Camera miniMapCamera;
    public int miniMapZoomSize = 15;
    public int zoom1;
    public int zoom2;
    public int zoom3;
    [Range(0, 2)]
    public int nextCamera;
    string cameraChangeButton = "m";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(cameraChangeButton))
        {
            if(nextCamera == 0)
            {
                miniMapZoomSize = zoom1;
                nextCamera = 1;
            }
            else if(nextCamera == 1)
            {
                miniMapZoomSize = zoom2;
                nextCamera = 2;
            }
            else if(nextCamera == 2)
            {
                miniMapZoomSize = zoom3;
                nextCamera = 0;
            }
        }
        miniMapCamera.orthographicSize = miniMapZoomSize;
    }
}
