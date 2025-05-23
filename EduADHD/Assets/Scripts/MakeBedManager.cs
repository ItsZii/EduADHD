using UnityEngine;

public class MakeBedManager : MonoBehaviour
{
    [Header("Blankets")]
    public GameObject madeBed;
    public GameObject unmadeBed;


    public void setUpBed()
    {
        if (MainManager.Instance.isBedMade)
        {
            madeBed.SetActive(true);
            unmadeBed.SetActive(false);
        }
        else
        {
            madeBed.SetActive(false);
            unmadeBed.SetActive(true);
        }
    }

    public void MakeBed()
    {
        MainManager.Instance.isBedMade = true;
        madeBed.SetActive(true);
        unmadeBed.SetActive(false);
    }
}
