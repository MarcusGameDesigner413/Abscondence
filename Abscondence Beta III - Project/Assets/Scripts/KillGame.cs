using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGame : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
<<<<<<< HEAD
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
=======
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");
>>>>>>> master
    }


}
