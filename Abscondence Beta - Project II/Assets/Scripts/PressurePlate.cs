using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public float delay;
    float timer;
    public GameObject[] roomPressurePlates;
    public GameObject deactivateTarget;
    bool roomCheckPass = true;
    public Material active;
    public Material unactive;
    AudioSource activateSound;
    [HideInInspector]
    public bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = unactive;
        activateSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Block")
            timer = delay;
        else if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            activated = true;
            gameObject.GetComponent<MeshRenderer>().material = active;
            activateSound.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Block")
        {
            timer -= Time.deltaTime;
            if (timer <= 0 && activated == false)
            {
                activated = true;
                activateSound.Play();
                gameObject.GetComponent<MeshRenderer>().material = active;
            }
            bool roomCheckPass = true;
            for (int i = 0; i < roomPressurePlates.Length; i++)
            {
                if (roomPressurePlates[i].GetComponent<PressurePlate>().activated == false)
                    roomCheckPass = false;


            }


            if (roomCheckPass == true)
                deactivateTarget.gameObject.SetActive(false);
            else if (roomCheckPass == false)
                deactivateTarget.gameObject.SetActive(true);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        timer = 0;
        activated = false;
        activateSound.Play();
        gameObject.GetComponent<MeshRenderer>().material = unactive;
        deactivateTarget.gameObject.SetActive(true);
    }
}



/*If block
 *  public float delay \/
 *  after delay change green \/
 *  removing instant red
 *If player or enemy
 * instantly on/off \/
 * 
 *Misc notes
 * All pressure plates need to be on Array of gameobjects \/
 * Deactivate an object \/
 * If one signal is off door turns on \/
 * Plays audio when on or off \/ */

