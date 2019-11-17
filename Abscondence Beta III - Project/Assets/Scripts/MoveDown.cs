using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDown : MonoBehaviour
{
    public GameObject panelThing;
    public int travelDistance;
    public float moveSpeed;

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = panelThing.transform.position;
        for (int i = 0; i < travelDistance; i++)
        {
            pos.y += moveSpeed;
            panelThing.transform.position = pos;
        }
    }
}
