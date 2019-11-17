using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AIDialogueBoxes : MonoBehaviour
{
    bool beenEntered = false;
    public string nextTextLine;
    public GameObject aI;
    public GameObject aIDialogueTextBox;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!beenEntered)
        {
            if (other.gameObject.tag == "Player")
            {
                aI.SetActive(true);
                aIDialogueTextBox.GetComponent<TextMeshProUGUI>().text = nextTextLine;
                beenEntered = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        aIDialogueTextBox.GetComponent<TextMeshProUGUI>().text = "";
        aI.gameObject.SetActive(false);
    }
}
