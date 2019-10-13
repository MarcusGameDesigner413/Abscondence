﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : MonoBehaviour
{
    Vector3 relativePosition;
    Vector3 lastFramePosition;
    Vector3 secondLastFramePosition;
    Vector3 thirdLastFramePosition;
    Vector3 fourthLastFramePosition;
    Vector3 fifthLastFramePosition;
    Vector3 sixthLastFramePosition;
    Vector3 seventhLastFramePosition;
    Quaternion targetRotation;
    LineRenderer line = null;
    public GameObject player;
    public GameObject turretBeam;
    public float outOfRangeRadius;
    public float inRangeRadius;
    public float cooldown;
    public float rotationSpeed;
    public float rotationTime = 0;
    public float lerpTimeRotated = 0;
    public float maxRotationTime;
    public float maxBeamDistance;
    public float yOffset;
    public float shootingTimer;
    public float maxShootingTime;
    public float knockBackForce = 10;
    [Range(1,2)]
    public int variant;
    public bool rotating = true;
    public bool shooting = false;
    public bool playerHasBeenDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        turretBeam.SetActive(false);
        line.enabled = false;
        if (Vector3.Distance(player.transform.position, transform.position) < outOfRangeRadius || playerHasBeenDetected)
        {
            RaycastHit playerCheckHit = new RaycastHit();
            if (Physics.Linecast(transform.position, player.transform.position, out playerCheckHit))
            {
                if (playerCheckHit.collider.tag == "Player")
                {
                    if (Vector3.Distance(player.transform.position, transform.position) < inRangeRadius)
                    {
                        relativePosition = player.transform.position - transform.position;
                        targetRotation = Quaternion.LookRotation(relativePosition);
                        rotationTime += Time.deltaTime;
                        lerpTimeRotated = rotationTime / maxRotationTime;
                    }
                    else if (playerHasBeenDetected)
                    {
                        relativePosition = seventhLastFramePosition - transform.position;
                        targetRotation = Quaternion.LookRotation(relativePosition);
                    }
                    playerHasBeenDetected = true;
                }
                if (playerHasBeenDetected)
                {
                    rotationTime += Time.deltaTime * rotationSpeed;
                    lerpTimeRotated = rotationTime / maxRotationTime;
                }
            }
            if (rotating)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpTimeRotated);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }

            if (rotationTime > maxRotationTime)
            {
                rotating = false;
                playerHasBeenDetected = false;
            }

            seventhLastFramePosition = sixthLastFramePosition;
            sixthLastFramePosition = fifthLastFramePosition;
            fifthLastFramePosition = fourthLastFramePosition;
            fourthLastFramePosition = thirdLastFramePosition;
            thirdLastFramePosition = secondLastFramePosition;
            secondLastFramePosition = lastFramePosition;
            lastFramePosition = player.transform.position;
        }

        if (!rotating)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit = new RaycastHit();

            Vector3[] linePos = new Vector3[3];
            linePos[0].x = transform.position.x;
            linePos[0].y = transform.position.y + yOffset;
            linePos[0].z = transform.position.z;

            if (!shooting)
            {
                shootingTimer = 0;
                shooting = true;
            }
            else
            {
                if (Physics.Raycast(ray, out hit, 9999))
                {
                    Vector3 dir = transform.forward;
                    linePos[1].x = hit.point.x;
                    linePos[1].y = hit.point.y + yOffset;
                    linePos[1].z = hit.point.z;
                    turretBeam.SetActive(true);
                    turretBeam.GetComponent<MeshRenderer>().enabled = false;
                    turretBeam.transform.position = hit.point;
                    line.SetPositions(linePos);
                    line.positionCount = 2;
                    line.enabled = true;
                    shootingTimer += Time.deltaTime;

                    if(hit.collider.tag == "Player")
                    {
                        hit.collider.GetComponent<CharacterController>().SimpleMove(transform.forward);
                        hit.collider.GetComponent<PlayerController>().maxHealth -= 5;
                        Debug.Log("Get Lazored Nerd");
                    }
                }
                if (shootingTimer > maxShootingTime)
                {
                    shooting = false;
                    rotating = true;
                    rotationTime = 0.0f;
                }
            }
        }
    }
}