using UnityEngine;
using TMPro;

public class TaskDisplay : MonoBehaviour
{
    public GameObject DoneTaskObj;
    private TextMeshProUGUI DoneTasks;

    public GameObject UnfinishedTaskObj;
    private TextMeshProUGUI UnfinishedTasks;

    private string textDoneT;
    private string textSkipT;
    void Start()
    {
        DoneTasks = DoneTaskObj.GetComponent<TextMeshProUGUI>();
        UnfinishedTasks = UnfinishedTaskObj.GetComponent<TextMeshProUGUI>();

        foreach (string task in MainManager.Instance.allTasks)
        {
            if (!MainManager.Instance.completedTasks.Contains(task))
            {
                MainManager.Instance.skippedTasks.Add(task);
            }
        }
        if (MainManager.Instance.completedTasks != null)
        {
            foreach (string task in MainManager.Instance.completedTasks)
            {
                textDoneT += "- " + task + "\n";
            }
            foreach (string task in MainManager.Instance.skippedTasks)
            {
                textSkipT += "- " + task + "\n";
            }
        }
        
        DoneTasks.SetText(textDoneT);
        UnfinishedTasks.SetText(textSkipT);
    }
}
