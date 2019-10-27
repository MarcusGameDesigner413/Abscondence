using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public string FirstLevelName;
    public string AbscondLevelName;
    public string CreditsLevelName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(FirstLevelName);
    }

    public void Abscond()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(AbscondLevelName);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Credits()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(CreditsLevelName);
    }
}
