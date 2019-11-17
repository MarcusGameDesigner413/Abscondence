using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffSwitch : MonoBehaviour
{
    
    public GameObject deactivateTarget;

    [HideInInspector]
    public bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            
            activated = true;
                
            if (activated == true)
            {
                deactivateTarget.gameObject.SetActive(true);
            }
              
        }
    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            activated = false;
            deactivateTarget.gameObject.SetActive(false);
        }
           
       
    }
}
