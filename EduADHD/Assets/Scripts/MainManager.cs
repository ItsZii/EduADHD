using UnityEngine;
using System.Collections.Generic;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public int SpoonCount;

    public float SocialStatus;
    public float EducationStatus;
    public float PersonalStatus;

    public bool isBedMade;
    public bool isMorning;

    public bool isPhone;
    public bool isBag;

    public List<string> allTasks, completedTasks, skippedTasks;
    public string location;

    private void Awake()
    {
        restartGame();

        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void restartGame()
    {
        SpoonCount = Random.Range(0, 21);
        SocialStatus = 50;
        EducationStatus = 50;
        PersonalStatus = 50;
        isMorning = true;
        isBedMade = false;
        isPhone = false;
        isBag = false;
        location = "Home";
        allTasks.Clear();
        completedTasks.Clear();
        skippedTasks.Clear();
    }
}