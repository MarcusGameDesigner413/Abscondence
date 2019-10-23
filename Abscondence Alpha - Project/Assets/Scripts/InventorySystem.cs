using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public GameObject player;
    Color orange = new Color(231.0f / 255.0f, 119.0f / 255.0f, 8.0f / 255.0f);
    Color red = new Color(236.0f / 255.0f, 0.0f / 255.0f, 7.0f / 255.0f);

    // Update is called once per frame
    void Update()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        var inventory = transform.Find("InventoryLine_1");

        inventory.Find("Medvial_Counter").GetComponent<TextMeshProUGUI>().text = "x" + playerController.storedMedvial;
        inventory.Find("Detpack_Counter").GetComponent<TextMeshProUGUI>().text = "x" + playerController.storedDetPack;
        inventory.Find("PowerCell_Counter").GetComponent<TextMeshProUGUI>().text = "x" + playerController.storedPowerCell;

        if(playerController.storedMedvial == playerController.maxMedvial)
        {
            inventory.Find("Medvial_Counter").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        }
        else if (playerController.storedMedvial <= playerController.maxMedvial)
        {
            inventory.Find("Medvial_Counter").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }
        if (playerController.storedDetPack == playerController.maxDetPack)
        {
            inventory.Find("Detpack_Counter").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        }
        else if (playerController.storedPowerCell <= playerController.maxDetPack)
        {
            inventory.Find("Detpack_Counter").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }
        if (playerController.storedPowerCell == playerController.maxPowerCell)
        {
            inventory.Find("PowerCell_Counter").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Bold;
        }
        else if (playerController.storedPowerCell <= playerController.maxPowerCell)
        {
            inventory.Find("PowerCell_Counter").GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;
        }

        if (playerController.storedMedvial == 0)
        {
            inventory.Find("Medvial_Counter").GetComponent<TextMeshProUGUI>().color = red;
        }
        else if (playerController.storedMedvial >= 0)
        {
            inventory.Find("Medvial_Counter").GetComponent<TextMeshProUGUI>().color = orange;
        }
        if (playerController.storedDetPack == 0)
        {
            inventory.Find("Detpack_Counter").GetComponent<TextMeshProUGUI>().color = red;
        }
        else if (playerController.storedPowerCell >= 0)
        {
            inventory.Find("Detpack_Counter").GetComponent<TextMeshProUGUI>().color = orange;
        }
        if (playerController.storedPowerCell == 0)
        {
            inventory.Find("PowerCell_Counter").GetComponent<TextMeshProUGUI>().color = red;
        }
        else if (playerController.storedPowerCell >= 0)
        {
            inventory.Find("PowerCell_Counter").GetComponent<TextMeshProUGUI>().color = orange;
        }


    }
}
