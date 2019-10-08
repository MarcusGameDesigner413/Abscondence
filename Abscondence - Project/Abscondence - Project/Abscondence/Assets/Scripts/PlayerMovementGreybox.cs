using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementGreybox : MonoBehaviour
{

    void Update() {
    if (Input.GetKey(KeyCode.W))
        {
            transform.position = transform.position + new Vector3(0, 0, 0.1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = transform.position + new Vector3(0, 0, -0.1f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = transform.position + new Vector3(-0.1f, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = transform.position + new Vector3(0.1f, 0, 0);
        }
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    transform.Rotate(transform.rotation.x, transform.rotation.y + 90f, transform.rotation.z);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    transform.Rotate(transform.rotation.x, transform.rotation.y - 90f, transform.rotation.z);
        //}
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    transform.Rotate(transform.rotation.x, transform.rotation.y - 90f, transform.rotation.z);
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    transform.Rotate(transform.rotation.x, transform.rotation.y + 90f, transform.rotation.z);
        //}
    }
}
