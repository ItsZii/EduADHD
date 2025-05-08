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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < spoons.Length; i++){
            int positiveSpoons = currentSpoons;

            if (currentSpoons < 0) {
                positiveSpoons = positiveSpoons * (-1);
            }
            if (i < positiveSpoons && currentSpoons > 0){
                spoons[i].sprite = unusedSpoon;
            } else if (i < positiveSpoons && currentSpoons < 0){
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
}
