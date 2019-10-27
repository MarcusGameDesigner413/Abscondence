using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardSystem : MonoBehaviour
{
    //should be set to 7
    public GameObject[] keyCardVisuals;
    
    //instance of player controller
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        //setup of the ploayer so we can see values
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if the player has a key
        if(player.hasKey == true)
        {
            //the keycard visual based on the keycard number the player has will be shown
            keyCardVisuals[player.keyType].SetActive(true);
        }
        else
        {
            //the keycard will not be shown
            keyCardVisuals[player.keyType].SetActive(false);
        }

        if (player.hasUniqueKey == true)
        {
            //the keycard visual based on the keycard number the player has will be shown
            keyCardVisuals[6].SetActive(true);
        }
        else
        {
            //the keycard will not be shown
            keyCardVisuals[6].SetActive(false);
        }


        // Set the images to display based on the amount of health the player has
        //for (int i = 0; i < player.currentHealth; i++)
        //{
        //    if (!(i < 0))
        //        keyCardVisuals[i].SetActive(true);
        //}

        //// If i is greater than the currentHealth, set the images beyond the currentHealth value to not display 
        //for (int i = player.currentHealth; i < keyCardVisuals.Length; i++)
        //{
        //    //if (!(i > 0))
        //    keyCardVisuals[i].SetActive(false);
        //}

        // Just in case for future use for updating array
        //maxHealthSize = player.maxHealth;
        //GameObject[] temp = new GameObject[maxHealthSize];
        //hearts.CopyTo(temp, 0);
        //hearts = temp;
    }
}
