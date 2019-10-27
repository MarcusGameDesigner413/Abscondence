using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapFollow : MonoBehaviour {

    public Transform player;

    void LateUpdate ()
    {
        //Camera follows the Player and doesn't rotate.
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }

}
