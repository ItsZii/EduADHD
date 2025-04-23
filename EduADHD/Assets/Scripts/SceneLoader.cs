using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void QuitGame()
    {
        print("Quiting game");
        Application.Quit();
    }
    public void StartGame()
    {
        print("Starting game");
        SceneManager.LoadScene("School");
    }
    public void AboutGame()
    {
        print("Switching to about game scene");
        SceneManager.LoadScene("AboutGame");
    }
    public void MainMenu()
    {
        print("Going to main menu");
        SceneManager.LoadScene("MainMenu");
    }
}
