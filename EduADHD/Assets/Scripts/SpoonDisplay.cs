using UnityEngine;
using UnityEngine.UI;

public class SpoonDisplay : MonoBehaviour
{
    public int currentSpoons;
    public int maxSpoons;

    public Sprite usedSpoon;
    public Sprite unusedSpoon;
    public Sprite debtSpoon;
    public Image[] spoons;
    [SerializeField] public StatusManager statusManager;

    void Update()
    {
        for(int i = 0; i < spoons.Length; i++){
            int positiveSpoons = MainManager.Instance.SpoonCount;

            if (MainManager.Instance.SpoonCount < 0) {
                positiveSpoons = positiveSpoons * (-1);
            }
            if (i < positiveSpoons && MainManager.Instance.SpoonCount > 0){
                spoons[i].sprite = unusedSpoon;
            } else if (i < positiveSpoons && MainManager.Instance.SpoonCount < 0){
                spoons[i].sprite = debtSpoon;
            } else {
                spoons[i].sprite = usedSpoon;
            }
            
            if(i < maxSpoons){
                spoons[i].enabled = true;
            } else{
                spoons[i].enabled = false;
            }
        }
    }
    public void UpdateSpoonCount(int changeAmount)
    {
        if (MainManager.Instance.SpoonCount < 0)
        {
            MainManager.Instance.SpoonCount -= changeAmount;
            statusManager.SetValue((-3 * changeAmount), "personal");
        } 
        else if (MainManager.Instance.SpoonCount - changeAmount < 0)
        {
            MainManager.Instance.SpoonCount -= changeAmount;
            statusManager.SetValue((MainManager.Instance.SpoonCount - changeAmount) * 3, "personal");
        }
        else
        {
            MainManager.Instance.SpoonCount -= changeAmount;
        }
    }
}
