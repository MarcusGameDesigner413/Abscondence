using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationKonami : MonoBehaviour
{
    public bool codeSuccess;
    public KeyCode[] keys = new KeyCode[]
    {
        KeyCode.A,
        KeyCode.B,
        KeyCode.S,
        KeyCode.C,
        KeyCode.O,
        KeyCode.N,
        KeyCode.D,
        KeyCode.E,
        KeyCode.N,
        KeyCode.C,
        KeyCode.E
    };

    IEnumerator Start()
    {
        float timer = 0f;
        int index = 0;

        while(true)
        {
            if(Input.GetKeyDown(keys[index]))
            {
                index++;
                if (index == keys.Length)
                {
                    codeSuccess = true;
                    timer = 0f;
                }
                else
                    timer = 0.75f;
            }
            timer -= Time.deltaTime;

            if(timer < 0)
            {
                timer = 0;
                index = 0;
            }

            yield return null;
        }
    }
}
