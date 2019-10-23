using System.Collections;
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
<<<<<<< HEAD
=======
    Vector3 eighthLastFramePosition;
    Vector3 ninthLastFramePosition;
    Vector3 tenthLastFramePosition;
>>>>>>> master
    Vector3 raycastPosition;
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
        raycastPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        turretBeam.SetActive(false);
        line.enabled = false;
        if (Vector3.Distance(player.transform.position, transform.position) < outOfRangeRadius || playerHasBeenDetected)
        {
            RaycastHit playerCheckHit = new RaycastHit();
            if (Physics.Linecast(raycastPosition, player.transform.position, out playerCheckHit))
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

<<<<<<< HEAD
=======
            tenthLastFramePosition = ninthLastFramePosition;
            ninthLastFramePosition = eighthLastFramePosition;
            eighthLastFramePosition = seventhLastFramePosition;
>>>>>>> master
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
            Ray ray = new Ray(raycastPosition, transform.forward);
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
                    linePos[1] = hit.point;
                    //turretBeam.SetActive(true);
                    //turretBeam.GetComponent<MeshRenderer>().enabled = false;
                    //turretBeam.transform.position = hit.point;
                    line.SetPositions(linePos);
                    line.positionCount = 2;
                    line.enabled = true;

                    if(hit.collider.tag == "Player")
                    {
                        hit.collider.GetComponent<CharacterController>().SimpleMove(transform.forward);
                        hit.collider.GetComponent<PlayerController>().currentHealth -= 1;
                        Debug.Log("Get Lazored Nerd");
                    }

                    if (hit.collider.tag == "Enemy")
                    {
                        //do enemy knockback
                        hit.collider.GetComponent<TrooperBehaviour>().wasDamaged = true;
                        hit.collider.GetComponent<TrooperBehaviour>().KnockBack(transform.forward.normalized);

                        hit.collider.GetComponent<TrooperBehaviour>().currentHealth -= sentryDamage;
                        Debug.Log("Get Lazored Lizard Boy");
                    }


                }
                if (shootingTimer > maxShootingTime)
                {
                    shooting = false;
                    rotating = true;
                    rotationTime = 0.0f;
                }
                shootingTimer += Time.deltaTime;
            }
        }
    }
    private void OnDrawGizmosSelected() //makes a sphare to match the size of the enemys "lookRadius" in the scene view
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, inRangeRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outOfRangeRadius);
    }

}
