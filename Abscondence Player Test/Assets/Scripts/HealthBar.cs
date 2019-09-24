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


        if (player.health < 0)
        {
            player.health = 0;
        }
        else
        {
            playerHealthBar.value = player.health;
            healthText.text = player.health.ToString();
        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            player.health += 10;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            player.health -= 10;
        }
    }
}
