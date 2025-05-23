using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class CornerTasksManager : MonoBehaviour
{
    public GameObject taskBoxObj;
    private TextMeshProUGUI taskBoxText;
    public List<string> totaltaskList = new List<string>();
    public List<string> visibleTaskList = new List<string>();

    IEnumerator Start()
    {
        yield return null; // ļauj MainManager pabeigt Start()

        taskBoxText = taskBoxObj.GetComponent<TextMeshProUGUI>();

        if (taskBoxText == null)
        {
            Debug.LogError("taskBoxText is null! Vai taskBoxObj satur TextMeshProUGUI?");
            yield break;
        }

        foreach (string task in MainManager.Instance.allTasks)
        {
            if (!MainManager.Instance.completedTasks.Contains(task))
            {
                totaltaskList.Add(task);
            }
        }

        Debug.Log("Loaded tasks: " + totaltaskList.Count);

        StartCoroutine(UpdateVisibleTasksRoutine());
    }

    private IEnumerator UpdateVisibleTasksRoutine()
    {
        while (true)
        {
            // Noņem visus izpildītos uzdevumus no saraksta
            totaltaskList.RemoveAll(task => MainManager.Instance.completedTasks.Contains(task));

            if (totaltaskList.Count > 0)
            {
                string randomTask = totaltaskList[Random.Range(0, totaltaskList.Count)];

                visibleTaskList.Add(randomTask);

                if (visibleTaskList.Count > 3)
                {
                    visibleTaskList.RemoveAt(0);
                }
                UpdateTaskBoxText();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdateTaskBoxText()
    {
        string textTask = "";

        foreach (string task in visibleTaskList)
        {
            textTask += "- " + task + "\n";
        }

        taskBoxText.SetText(textTask);
    }
}