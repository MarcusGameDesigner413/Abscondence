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
    Vector3 eightLastFramePosition;
    Vector3 nineLastFramePosition;
    Vector3 tenthLastFramePosition;
    Vector3 raycastPosition;
    Quaternion targetRotation;
    LineRenderer line = null;
    public GameObject player;
    public GameObject turretBeam;
    public float outOfRangeRadius;
    public float inRangeRadius;
    public float cooldown;
    public float rotationSpeed;
    float rotationTime = 0;
    float lerpTimeRotated = 0;
    public float maxRotationTime;
    public float maxBeamDistance;
    public float yOffset;
    float shootingTimer;
    public float maxShootingTime;
    public float knockBackForce = 10;
    public float damageRestTime;
    float damageRestTimer;
    public int sentryDamage;
    public int sentryDamageToTrooper;
    [Range(1,2)]
    public int variant;
    bool rotating = true;
    bool shooting = false;
    bool playerHasBeenDetected = false;
    public float sentryHealth;
    public ParticleSystem[] particles;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        raycastPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        if(variant == 2)
        {
            transform.Find("Shield").gameObject.SetActive(true);
            sentryHealth = 999;
            transform.GetComponent<BoxCollider>().enabled = false;
        }

        for (int i = 0; i < particles.Length; i++)
        {
            //if (!(i > 0))
            particles[i].Clear();
            particles[i].Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        turretBeam.SetActive(false);
        line.enabled = false;
        damageRestTimer -= Time.deltaTime;
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
                        relativePosition = tenthLastFramePosition - transform.position;
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

            tenthLastFramePosition = nineLastFramePosition;
            nineLastFramePosition = eightLastFramePosition;
            eightLastFramePosition = seventhLastFramePosition;
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
                    shootingTimer += Time.deltaTime;

                    if(hit.collider.tag == "Player")
                    {
                        hit.collider.GetComponent<CharacterController>().SimpleMove(transform.forward * knockBackForce);

                        if(damageRestTimer <= 0)
                        {
                            hit.collider.GetComponent<PlayerController>().currentHealth -= sentryDamage;
                            damageRestTimer = damageRestTime;
                            hit.collider.GetComponent<PlayerController>().PlayerTookDamageAudio.Play();
                        }

                        Debug.Log("Get Lazored Nerd");
                    }

                    if (hit.collider.tag == "Enemy")
                    {
                        //do enemy knockback
                        hit.collider.GetComponent<TrooperBehaviour>().wasDamaged = true;
                        hit.collider.GetComponent<TrooperBehaviour>().KnockBack(transform.forward.normalized);

                        hit.collider.GetComponent<TrooperBehaviour>().currentHealth -= sentryDamageToTrooper;
                        Debug.Log("Get Lazored Lizard Boy");
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
        if(sentryHealth <= 0)
        {
            //turn on cold particles
            for (int i = 0; i < particles.Length; i++)
            {
                //if (!(i > 0))
                particles[i].Play();
                //DeathParticles[i].Stop(true);
            }
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected() //makes a sphare to match the size of the enemys "lookRadius" in the scene view
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, inRangeRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, outOfRangeRadius);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Sword" && sentryHealth != 0)
        {
            float healthLostOnHit = 0;
            PlayerController playerController = player.GetComponent<PlayerController>();

            if(playerController.lightAttackUsed)
            {
                healthLostOnHit = playerController.playerLightDamage;
            }
            else if(playerController.heavyAttackUsed)
            {
                healthLostOnHit = playerController.playerHeavyDamage;
            }
            sentryHealth -= healthLostOnHit;
        }
    }
}
