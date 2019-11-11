using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWheel : MonoBehaviour
{
    public SpawnDoor spawner;
    public float interactTime = 2.0f;

    [HideInInspector]
    public bool wheelSpinComplete = false;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<SpawnDoor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wheelSpinComplete)
        {
            spawner.alwaysActive = false;
            spawner.doorLocked = true;
        }
    }
}
