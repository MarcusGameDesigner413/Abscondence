using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    //SOMETHING TO NOTE: 'Panel' tag required on object

    //DESIGNER DO NOT MODIFY OR ELSE OBJECT WILL BE DESTROYED ON STARTUP
    public bool activated = false;

    //this exists so the code doesnt trigger more than once
    private bool wasActivated = false;

    //this is the item that will be deleted
    public GameObject door;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the panel has been activated only on the first time
        if(activated && !wasActivated)
        {
            //destroy the door
            Destroy(door);

            //this gets set to true so the code DOES NOT COMPILE AGAIN
            wasActivated = true;
        }



    }
}
