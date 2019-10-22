using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySystem : MonoBehaviour
{
    public GameObject player;

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

    }
}
