using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TaskRegManager : MonoBehaviour
{
    public GameObject[] sceneTasks;
    public string type;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (MainManager.Instance.location != type) return;
        
        switch (MainManager.Instance.location)
        {
            case "Home": case "School":
                for (int i = 0; i < sceneTasks.Length; i++)
                {
                    MainManager.Instance.allTasks.Add(sceneTasks[i].gameObject.GetComponent<ObjectInteraction>().taskDesc);
                }
                break;

            case "HomeEnd":
                for (int i = 0; i < sceneTasks.Length; i++)
                {
                    MainManager.Instance.allTasks.Add(sceneTasks[i].gameObject.GetComponent<ObjectInteraction>().taskDesc2);
                }
                break;
        }
    }
}
