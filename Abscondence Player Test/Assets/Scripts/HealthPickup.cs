using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{

    public int healthRestoreAmount = 25;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Collider>().isTrigger = enabled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
