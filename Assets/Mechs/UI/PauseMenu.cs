using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;

    public GameObject pauseUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                pauseUI.SetActive(true);
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        SC_FPSController.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        SC_FPSController.canMove = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void Settings()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
