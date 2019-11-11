using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameObject pauseMenu;

    bool Paused;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (Paused) //not paused
        {
            Paused = false;
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            playerController.gamePaused = false;
            pauseMenu.SetActive(false);
        }
        else
        { // game is paused
            Paused = true;
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            playerController.gamePaused = true;
            pauseMenu.SetActive(true);
        }
    }

    public void Resume()
    {
        Pause();
    }

    public void Quit()
    {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
