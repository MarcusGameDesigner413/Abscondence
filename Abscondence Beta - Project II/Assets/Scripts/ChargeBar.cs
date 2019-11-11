using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    public RawImage chargeBlock;
    public float maxScale = 2.714f; // Max scale of the box inside the bar

    private float minScale = 0.0f;
    private float keyHoldTimer = 0f;
    private Vector3 chargeBarScale;
    private Vector3 temp;
    private PlayerController player;
    private SpinWheel spin;


    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        spin = GetComponent<SpinWheel>();
        chargeBarScale = chargeBlock.rectTransform.localScale;
        temp = new Vector3(1.0f, keyHoldTimer, 1.0f);
        chargeBarScale.y = minScale;
    }

    // Update is called once per frame
    void Update()
    {
        switch (player.chargingType)
        {
            case "Health":
                // Math to make the timer match the scale of the bar
                keyHoldTimer = (player.medkitScavengeTimer / 2) * maxScale;
                break;
            case "SpinWheel":
                    keyHoldTimer = (player.spinWheelTimer / 2) * maxScale;
                break;
            default:
                keyHoldTimer = 0.0f;
                break;
        }

        temp.y = keyHoldTimer;

        if (player.chargingType == "Health")
            temp.y = maxScale - temp.y; // Invert the block

        // Boundary check
        if (temp.y > maxScale)
            temp.y = maxScale;

        chargeBlock.rectTransform.localScale = temp;
    }

}
