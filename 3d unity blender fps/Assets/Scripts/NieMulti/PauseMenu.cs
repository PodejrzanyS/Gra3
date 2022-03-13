using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Resume();
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;

                Pause();
            }
        }
    }


    void Resume()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;


    }

    void Pause()
    {

        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(0);
        
    }
    public void Resumee()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
}
