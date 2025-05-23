using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseController : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioSource[] audios;

    private bool _gamePaused = false;
    public bool IsPaused => _gamePaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _gamePaused == false)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _gamePaused == true)
        {
            ResumeGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        _gamePaused = false;
        if (GameObject.FindGameObjectWithTag("Player") != null) GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        if (GameObject.FindGameObjectWithTag("Player2") != null) GameObject.FindGameObjectWithTag("Player2").GetComponent<FirstPersonController>().enabled = true;

        for (int i = 0; i < audios.Length; i++)
            {
                audios[i].UnPause();
            }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        _gamePaused = true;
        if (GameObject.FindGameObjectWithTag("Player") != null) GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        if (GameObject.FindGameObjectWithTag("Player2") != null) GameObject.FindGameObjectWithTag("Player2").GetComponent<FirstPersonController>().enabled = false;
        for (int i = 0; i < audios.Length; i++)
        {
            audios[i].Pause();
        }
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Home()
    {
        pauseMenu.SetActive(false);
        _gamePaused = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
    public void QuitGame()
    {
        print("Quiting game");
        Application.Quit();
    }
}
