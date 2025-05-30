using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;
    public GameObject pauseMenu;
    public FPSMovement FPSMovement;

    private bool isPaused = false;

    void Start()
    {
        Resume(); // Start the game in a non-paused state
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Resume called");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        FPSMovement.Resume();
   
    }

    public void Pause()
    {
        Debug.Log("Pause called");
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        FPSMovement.Pause();
    }

}