using UnityEngine;

public class EveningDishes : MonoBehaviour
{
    public GameObject objects;
    void Start()
    {
        if (!MainManager.Instance.isMorning)
        {
            objects.SetActive(true);
        }
    }
}
