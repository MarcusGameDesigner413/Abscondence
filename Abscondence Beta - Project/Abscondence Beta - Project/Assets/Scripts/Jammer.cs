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

    [HideInInspector]
    public bool isJamming = true;

    //setup for explosion
    private bool isGonnaBlow = false;

    //game object for the static object
    public GameObject StaticJam;

    //game object forthe background object
    public GameObject BackgroundJam;

    //should be copy of detpack model that spawns on 
    public GameObject DetPack;

    //array of cold particle effects (used for when alive)
    public ParticleSystem[] ColdParticles;

    //array of smoke particle effects (used when dead)
    public ParticleSystem[] DeathParticles;

    

    // Start is called before the first frame update
    void Start()
    {
        //turn death particles off
        for (int i = 0; i < DeathParticles.Length; i++)
        {
            //if (!(i > 0))
            DeathParticles[i].Clear();
            DeathParticles[i].Stop();
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
           

            //its gonna blow,
            isGonnaBlow = true;
            DetPack.SetActive(true);
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

                DetPack.SetActive(false);

                StaticJam.SetActive(false);
                BackgroundJam.SetActive(false);

                //do whatever effects i guess
                for (int i = 0; i < DeathParticles.Length; i++)
                {
                    //if (!(i > 0))
                    DeathParticles[i].Play();
                    //DeathParticles[i].Stop(true);
                }

                //made to prevent code running further
                isGonnaBlow = false;
            }
            
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
