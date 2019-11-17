using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakWallDestroy : MonoBehaviour
{
    //the new room that will be shown afer the room is gone bye bye
    public GameObject newRoomToSpawn;

    //the detpack that will be shown on wall during explosion
    public GameObject detPack;

    //the wall itself
    public GameObject oldSealedWall;

    //the new broken wall
    public GameObject brokenWall;

    //array of particle effects (used for when blowing up)
    public ParticleSystem[] boomParticles;

    //audio queue here

    //seconds until datapack explosion
    public float secondsToDestroy = 5;

    //iteration in explosion
    private float destroyIterator = 0;

    //bool to deal with being broken
    private bool isBroken = false;

    //setup for explosion
    [HideInInspector]
    public bool isGonnaBlow = false;


    public float explosionTime = 3;

    private float explosionIterator = 0;

    [HideInInspector]
    public bool isExploding = false;

    [HideInInspector]
    public bool hasExploded = false;

    public SphereCollider explosionRadius;

    public int explosionDamageAmount = 1;

    public int knockBackForce = 1;

    private bool playerhit = false;

    //sound effect 1 (explosion)

    //sound effect 2 (tahdddddduuuuuuuuuuuuuuuuuuh)

    // Start is called before the first frame update
    void Start()
    {
        //turn death particles off
        for (int i = 0; i < boomParticles.Length; i++)
        {
            //if (!(i > 0))
            boomParticles[i].Clear();
            boomParticles[i].Stop();
        }

        oldSealedWall.SetActive(true);
        brokenWall.SetActive(false);
        newRoomToSpawn.SetActive(false);
        detPack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if you are going to explode but havent yet
        if (isGonnaBlow && !isBroken)
        {
            detPack.SetActive(true);

            //timer to explode
            destroyIterator += 1 * Time.deltaTime;

            if (destroyIterator >= secondsToDestroy)
            {


                detPack.SetActive(false);

                

                //do whatever effects i guess
                for (int i = 0; i < boomParticles.Length; i++)
                {
                    //if (!(i > 0))
                    boomParticles[i].Play();
                    //DeathParticles[i].Stop(true);
                }

                oldSealedWall.SetActive(false);
                brokenWall.SetActive(true);
                newRoomToSpawn.SetActive(true);

                //made to prevent code running further
                isBroken = true;
                isExploding = true;
                


            }


        }

        //exploding now
        if (isExploding && !hasExploded)
        {
            explosionIterator += 1 * Time.deltaTime;

            if (explosionIterator >= explosionTime && isExploding)
            {
                //explosionRadius.enabled = !explosionRadius.enabled;
                //Debug.Log("Explosion sphere should be off");
                //made to prevent code running further
                hasExploded = true;
                isExploding = false;


                //sound effect 2 (tahdddddduuuuuuuuuuuuuuuuuuh)

                //StaticJam.SetActive(false);
                //BackgroundJam.SetActive(false);
                //PostProcessingEffect.SetActive(false);

                //turn off boom particles
                for (int i = 0; i < boomParticles.Length; i++)
                {
                    //if (!(i > 0))
                    boomParticles[i].Clear();
                    boomParticles[i].Stop();
                }


                explosionRadius.enabled = false;





            }

        }

    }

    private void OnTriggerStay(Collider other)
    {
        //settup damage for player, enemy and turret
        if (isExploding && !hasExploded)
        {
            //damage enemy
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<TrooperBehaviour>().wasDamaged = true;
                other.gameObject.GetComponent<TrooperBehaviour>().KnockBack(transform.forward.normalized);

                other.gameObject.GetComponent<TrooperBehaviour>().currentHealth -= explosionDamageAmount;
            }

            //damage player
            if (other.gameObject.tag == "Player" && !playerhit)
            {
                other.gameObject.GetComponent<CharacterController>().SimpleMove(transform.forward * knockBackForce);
                other.gameObject.GetComponent<PlayerController>().currentHealth -= explosionDamageAmount;

            }

            //damage turret
            if (other.gameObject.tag == "Sentry")
            {
                other.gameObject.GetComponent<Sentry>().sentryHealth -= explosionDamageAmount;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerhit = false;
        }
    }
}
