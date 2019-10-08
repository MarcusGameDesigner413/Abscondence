using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider playerHealthBar;
    public Text healthText;
    PlayerController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {


        if (player.currentHealth < 0)
        {
            player.currentHealth = 0;
        }
        else
        {
            playerHealthBar.value = player.currentHealth;
            healthText.text = player.currentHealth.ToString();
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            player.currentHealth += 10;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            player.currentHealth -= 10;
        }
    }
}
