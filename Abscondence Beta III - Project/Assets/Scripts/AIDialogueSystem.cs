using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AIDialogueSystem : MonoBehaviour
{
<<<<<<< HEAD
    public string[] nextTextLine;
    public GameObject aI;
    public GameObject aIDialogueTextBox;
    public GameObject medKitPickup;
    public GameObject batonPickup;
    public GameObject lockerSearch;
    public GameObject dummyLockerSearch;
    public ObjectLooting createSearch;
    public Panel doorPanel;
    public Panel doorPanel2;
    public CardPanel doorKeyPanel;
    public GameObject blockingBoxes;
    public TrooperBehaviour firstEnemy;
    public float textTimerLength = 5f;

    private int counter = 0;
    private TextMeshProUGUI aiDialogue;
    private float timer = 0f;
    private bool[] startText = { false };
    private bool timerStarted = true;
    private bool gamePaused = false;
    private bool cameraUsed = false;
    private BoxCollider locker1;
    private BoxCollider locker2;
    private TutorialTriggerBox triggerBox1;
    private TutorialTriggerBox triggerBox2;
    private TutorialTriggerBox triggerBox3;
    private TutorialTriggerBox triggerBox4;
    private TutorialTriggerBox triggerBox5;
    private TutorialTriggerBox triggerBox6;

    // Start is called before the first frame update
    void Start()
    {
        //medKitPickup = GameObject.Find("Medkit_Small");
        startText = new bool[nextTextLine.Length];
        timer = textTimerLength;
        aiDialogue = aIDialogueTextBox.GetComponentInChildren<TextMeshProUGUI>();
        lockerSearch.GetComponent<BoxCollider>().enabled = false;
        dummyLockerSearch.GetComponent<BoxCollider>().enabled = false;
        triggerBox1 = GameObject.Find("TriggerBox (1)").GetComponent<TutorialTriggerBox>();
        triggerBox2 = GameObject.Find("TriggerBox (2)").GetComponent<TutorialTriggerBox>();
        triggerBox3 = GameObject.Find("TriggerBox (3)").GetComponent<TutorialTriggerBox>();
        triggerBox4 = GameObject.Find("TriggerBox (4)").GetComponent<TutorialTriggerBox>();
        triggerBox5 = GameObject.Find("TriggerBox (5)").GetComponent<TutorialTriggerBox>();
        triggerBox6 = GameObject.Find("TriggerBox (6)").GetComponent<TutorialTriggerBox>();
        triggerBox4.GetComponent<BoxCollider>().enabled = false;
=======
    bool beenEntered = false;
    public string nextTextLine;
    public GameObject aI;
    public GameObject aIDialogueTextBox;
    // Start is called before the first frame update
    void Start()
    {
        
>>>>>>> master
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        // Debug Destroy baton
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            Destroy(batonPickup);
        }

        if (timerStarted)
            timer -= Time.deltaTime;

        // Starting text
        if (!startText[0])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[0]);

            if (triggerBox1.playerExitedTrigger)
                RemoveText();
            else
                RemoveTextAfterDelay();
        }
        else if (startText[0] && !startText[1])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[1]);

            if (triggerBox2.playerEnteredTrigger)
                RemoveText();
        }
        else if (triggerBox2.playerEnteredTrigger && medKitPickup != null)
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[2]);

            if (medKitPickup == null)
                RemoveText();
        }
        else if (medKitPickup == null && !startText[2])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[3]);
            RemoveTextAfterDelay();
        }
        else if (startText[2] && batonPickup != null)
        {
            Debug.Log("Baton picked up");
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[4]);

            // Check for baton picked up
            if (batonPickup == null)
                RemoveText();
        }
        else if (batonPickup == null && !startText[3])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[5]);
            RemoveTextAfterDelay();
            lockerSearch.GetComponent<BoxCollider>().enabled = true;
            dummyLockerSearch.GetComponent<BoxCollider>().enabled = true;
        }
        else if (startText[3] && !startText[4])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[6]);

            if (lockerSearch.gameObject.GetComponent<ObjectLooting>().searched)
                RemoveText();
        }
        else if (lockerSearch.gameObject.GetComponent<ObjectLooting>().searched && !startText[5])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[7]);

            if (doorPanel.wasActivated == true)
                RemoveText();
        }
        else if (doorPanel.wasActivated && startText[5] && !startText[6])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[8]);
            RemoveTextAfterDelay();
        }
        else if (triggerBox3.playerEnteredTrigger && !startText[7])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[9]);

            if (firstEnemy.xIsDownedX)
                RemoveText();
        }
        else if (firstEnemy.xIsDownedX && !startText[8])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[10]);

            if (firstEnemy.xIsDeadX)
                RemoveText();
        }
        else if (firstEnemy.xIsDeadX && !startText[9])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[11]);
            triggerBox4.GetComponent<BoxCollider>().enabled = true;

            if (triggerBox4.playerEnteredTrigger && firstEnemy.xIsDeadX)
                RemoveText();
            else
                RemoveTextAfterDelay();
        }
        else if (triggerBox4.playerEnteredTrigger && firstEnemy.xIsDeadX && !startText[10])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[12]);

            if (doorPanel2.wasActivated)
                RemoveText();
        }
        else if (doorPanel2.wasActivated && !startText[11])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[13]);

            if (createSearch.searched)
                RemoveText();
        }
        else if (createSearch.searched && !startText[12])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[14]);

            if (doorKeyPanel.wasActivated)
                RemoveText();
        }
        else if (doorKeyPanel.wasActivated && triggerBox5.playerEnteredTrigger && !startText[13])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[15]);
            blockingBoxes.SetActive(true);

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.JoystickButton9))
            {
                blockingBoxes.gameObject.SetActive(false);
                cameraUsed = true;
                RemoveText();
            }
        }
        else if (cameraUsed && !startText[14])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[16]);
            RemoveTextAfterDelay();
        }
        else if (triggerBox6.playerEnteredTrigger && !startText[15])
        {
            aI.SetActive(true);
            aiDialogue.SetText(nextTextLine[17]);
            RemoveTextAfterDelay();
        }
    }
    private void RemoveText()
    {
        aiDialogue.SetText("");
        aI.gameObject.SetActive(false);

        counter += 1;

        if (counter < nextTextLine.Length)
        {
            for (int i = 0; i < counter; i++)
            {
                startText[i] = true;
=======
        
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
>>>>>>> master
            }
        }
    }

<<<<<<< HEAD
    private void RemoveTextAfterDelay()
    {
        timerStarted = true;

        if (timer < 0f)
        {
            aiDialogue.SetText("");
            aI.gameObject.SetActive(false);
            timerStarted = false;
            timer = textTimerLength;
            counter += 1;

            if (counter < nextTextLine.Length)
            {
                for (int i = 0; i < counter; i++)
                {
                    startText[i] = true;
                }
            }
        }
    }

}
=======
    private void OnTriggerExit(Collider other)
    {
        aIDialogueTextBox.GetComponent<TextMeshProUGUI>().text = "";
        aI.gameObject.SetActive(false);
    }
}
>>>>>>> master
