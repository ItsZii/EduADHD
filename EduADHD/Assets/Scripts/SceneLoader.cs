using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public ObjectInteraction[] interactObjects;
    private Coroutine _currentCoroutine;
    private string currentSceneName;

    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }
    void Update()
    {
        if (interactObjects == null) return;

        foreach (ObjectInteraction interactObject in interactObjects)
        {
            if (interactObject.IsLeavingSchool)
            {
                print("Going home");
                SceneManager.LoadScene("Home");
                MainManager.Instance.location = "HomeEnd";
            }
            else if (interactObject.IsLeavingHome)
            {
                print("Going to school");
                SceneManager.LoadScene("School");
                MainManager.Instance.location = "School";
            }
            else if (interactObject.IsGoingSleep)
            {
                print("Game end");
                // Only here we go through Interval scene
                MainManager.Instance.location = "Home";
                SceneManager.LoadScene("Interval");
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        // Handle Interval scene logic
        if (currentSceneName == "Interval" && MainManager.Instance.location == "Home")
        {
            if (_currentCoroutine == null)
            {
                _currentCoroutine = StartCoroutine(IntervalWait("End"));
            }
        }
    }


    private IEnumerator IntervalWait(string sceneName)
    {
        yield return new WaitForSeconds(15);
        SceneManager.LoadScene(sceneName);

        MainManager.Instance.location = "Home";
    }

    public void QuitGame()
    {
        print("Quiting game");
        Application.Quit();
    }
    public void StartGame()
    {
        print("Starting game");
        MainManager.Instance.restartGame();
        SceneManager.LoadScene("Home");
    }
    public void AboutGame()
    {
        print("Switching to about game scene");
        SceneManager.LoadScene("AboutGame");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void MainMenu()
    {
        print("Going to main menu");
        SceneManager.LoadScene("MainMenu");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
