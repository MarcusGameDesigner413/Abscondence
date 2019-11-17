using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    //SOMETHING TO NOTE: 'Panel' tag required on object

    //DESIGNER DO NOT MODIFY OR ELSE OBJECT WILL BE DESTROYED ON STARTUP
    [HideInInspector]
    public bool xActivatedX = false;

    //this exists so the code doesnt trigger more than once
    [HideInInspector]
    public bool wasActivated = false;

    //this is the item that will be deleted
    public GameObject oldDoor;

    //powercell visual to appear
    public GameObject newPowerCell;

    public float SecondsToDestroy = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        newPowerCell.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if the panel has been activated only on the first time
        if(xActivatedX && !wasActivated)
        {
            
            //destroy the old door after the amount of time
            Destroy(oldDoor, SecondsToDestroy);

            //play the animation during time
            oldDoor.gameObject.GetComponent<Animator>().SetTrigger("TriggerFade");

            //make visual powercell exist
            newPowerCell.SetActive(true);

            //this gets set to true so this code DOES NOT COMPILE AGAIN
            wasActivated = true;

            
        }
    }
}
