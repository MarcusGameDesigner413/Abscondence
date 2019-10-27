using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarSystem : MonoBehaviour
{
    public GameObject[] hearts;
    private int maxHealthSize;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        maxHealthSize = player.maxHealth;

        // Set the images to display based on the amount of health the player has
        for (int i = 0; i < player.currentHealth; i++)
        {
            if (!(i < 0))
                hearts[i].SetActive(true);
        }

        // If i is greater than the currentHealth, set the images beyond the currentHealth value to not display 
        for (int i = player.currentHealth; i < hearts.Length; i++)
        {
            //if (!(i > 0))
                hearts[i].SetActive(false);
        }

        // Just in case for future use for updating array
        //maxHealthSize = player.maxHealth;
        //GameObject[] temp = new GameObject[maxHealthSize];
        //hearts.CopyTo(temp, 0);
        //hearts = temp;
    }
}
