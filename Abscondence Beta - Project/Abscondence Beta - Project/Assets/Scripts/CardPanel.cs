using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPanel : MonoBehaviour
{
    //SOMETHING TO NOTE: 'CardPanel' tag required on object

    //DESIGNER DO NOT MODIFY OR ELSE OBJECT WILL BE DESTROYED ON STARTUP
    [HideInInspector]
    public bool xActivatedX = false;

    //this exists so the code doesnt trigger more than once
    [HideInInspector]
    private bool wasActivated = false;

    //this is the item that will be deleted
    public GameObject oldDoor;

    public bool requiresMaster = false;

    public float SecondsToDestroy = 1;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if the panel has been activated only on the first time
        if (xActivatedX && !wasActivated)
        {

            //destroy the old door after the amount of time
            Destroy(oldDoor, SecondsToDestroy);

            //play the animation during time
            oldDoor.gameObject.GetComponent<Animator>().SetTrigger("TriggerFade");

            //this gets set to true so this code DOES NOT COMPILE AGAIN
            wasActivated = true;


        }
    }
}
