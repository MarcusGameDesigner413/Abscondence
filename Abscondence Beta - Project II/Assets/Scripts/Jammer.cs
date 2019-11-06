using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jammer : MonoBehaviour
{
    //you be the big radius
    public float staticJamRadius = 20;

    //just background radius
    public float backgroundJamRadius = 18;

    //seconds until datapack explosion
    public float secondsToDestroy = 5;

    //iteration in explosion
    private float destroyIterator = 0;

    //[HideInInspector]
    public bool isJamming = true;

    //setup for explosion
    [HideInInspector]
    public bool isGonnaBlow = false;

    //game object for the static object
    public GameObject StaticJam;

    //game object forthe background object
    public GameObject BackgroundJam;

    //should be copy of detpack model that spawns on 
    public GameObject DetPack;

    //array of cold particle effects (used for when alive)
    public ParticleSystem[] ColdParticles;

    //array of cold particle effects (used for when alive)
    public ParticleSystem[] boomParticles;

    //array of smoke particle effects (used when dead)
    public ParticleSystem[] DeathParticles;

    //because connor wanted it
    public GameObject PostProcessingEffect;

    

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
        explosionRadius.enabled = false;

        //turn death particles off
        for (int i = 0; i < DeathParticles.Length; i++)
        {
            //if (!(i > 0))
            DeathParticles[i].Clear();
            DeathParticles[i].Stop();
        }

        //turn death particles off
        for (int i = 0; i < boomParticles.Length; i++)
        {
            //if (!(i > 0))
            boomParticles[i].Clear();
            boomParticles[i].Stop();
        }

        //turn on cold particles
        for (int i = 0; i < ColdParticles.Length; i++)
        {
            //if (!(i > 0))
            ColdParticles[i].Play();
            //DeathParticles[i].Stop(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if you is jamming
        if(isJamming && !isGonnaBlow)
        {
            
            Vector3 playerPos = GameObject.Find("Player").GetComponent<Transform>().position;

            float distance = Vector3.Distance(playerPos, transform.position);
            //if inside the radius
            if (distance <= staticJamRadius)
            {
                //you be jamming
                StaticJam.SetActive(true);
            }
            else
            {
                //the gig is up... for now
                StaticJam.SetActive(false);
            }

            //if inside other radius
            if (distance <= backgroundJamRadius)
            {
                //you be jamming
                BackgroundJam.SetActive(true);
            }
            else
            {
                //the gig is up... for now
                BackgroundJam.SetActive(false);
            }
        }
        else
        {
            //if you have not exploded
            if(!hasExploded)
            {
                //its gonna blow,
                isGonnaBlow = true;
                DetPack.SetActive(true);
                //play sound 1
            }

        }

        if(isGonnaBlow)
        {
            

            //timer to explode
            destroyIterator += 1 * Time.deltaTime;

            if(destroyIterator >= secondsToDestroy)
            {
                //turn off the cold
                for (int i = 0; i < ColdParticles.Length; i++)
                {
                    //if (!(i > 0))
                    ColdParticles[i].Clear();
                    ColdParticles[i].Stop();
                }

               //if(!isExploding && isGonnaBlow)
                //{
                    //turn on explosion particles
                    for (int i = 0; i < boomParticles.Length; i++)
                    {
                        //if (!(i > 0))
                        boomParticles[i].Play();
                        //DeathParticles[i].Stop(true);
                    }
                    Debug.Log("BOOM PARTICLES SHOULD BE HERE");
                //}
                

                

                DetPack.SetActive(false);

                isExploding = true;

                explosionRadius.enabled = true;

                //made to prevent code running further
                isGonnaBlow = false;
            }
            
        }

        //exploding now
        if(isExploding && !hasExploded)
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

                StaticJam.SetActive(false);
                BackgroundJam.SetActive(false);
                PostProcessingEffect.SetActive(false);

                //turn off boom particles
                for (int i = 0; i < boomParticles.Length; i++)
                {
                    //if (!(i > 0))
                    boomParticles[i].Clear();
                    boomParticles[i].Stop();
                }



                //do whatever effects i guess
                for (int i = 0; i < DeathParticles.Length; i++)
                {
                    //if (!(i > 0))
                    DeathParticles[i].Play();
                    //DeathParticles[i].Stop(true);
                }
                Debug.Log("Death particles should be playing");



            }

        }
        else
        {
            explosionRadius.enabled = false;
        }
       

        //ensures explosion is off (it wasnt turning off for some reason
        //if(hasExploded)
        //{
        //    explosionRadius.enabled = false;
        //    Debug.Log("Explosion sphere should be off");
        //}

    }

    private void OnTriggerStay(Collider other)
    {
        //settup damage for player, enemy and turret
        if (isExploding && !hasExploded)
        {
            //damage enemy
            if(other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<TrooperBehaviour>().wasDamaged = true;
                other.gameObject.GetComponent<TrooperBehaviour>().KnockBack(transform.forward.normalized);

                other.gameObject.GetComponent<TrooperBehaviour>().currentHealth -= explosionDamageAmount;
            }

            //damage player
            if(other.gameObject.tag == "Player" && !playerhit)
            {
                other.gameObject.GetComponent<CharacterController>().SimpleMove(-transform.forward * knockBackForce);
                other.gameObject.GetComponent<PlayerController>().currentHealth -= explosionDamageAmount;

                playerhit = true;
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


    private void OnDrawGizmosSelected() //makes a sphare to match the size of the enemys "JamRadius" in the scene view
    {
       
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(transform.position, staticJamRadius);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, backgroundJamRadius);


    }
}
