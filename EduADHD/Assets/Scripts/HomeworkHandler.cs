using UnityEngine;
using System.Collections;

public class HomeworkHandler : MonoBehaviour
{
    public GameObject mainPlayer;
    public GameObject sittingPlayer;
    public AudioSource[] interactAudios;
    public GameObject instructionUI;

    [SerializeField] private SkillCheckSlider skillCheckManager;
    [SerializeField] public StatusManager statusManager;

    private bool isFinished;
    private int count;
    private Coroutine skillCheckLoop;

    void Start()
    {
        isFinished = false;

        // Switch to sitting state
        mainPlayer.SetActive(false);
        sittingPlayer.SetActive(true);

        // Start the loop only if it's evening
        if (!MainManager.Instance.isMorning)
        {
            skillCheckLoop = StartCoroutine(SkillCheckRepeater());
        }
    }

    IEnumerator SkillCheckRepeater()
    {
        while (!isFinished)
        {
            float waitTime = Random.Range(5f, 10f);
            yield return new WaitForSeconds(waitTime);

            if (!isFinished)
            {
                instructionUI?.SetActive(true);
                skillCheckManager.ActivateHW(this);
            }
        }
    }

    public void ReportSkillCheckResult(bool success)
    {
        if (success)
        {
            count++;
            if (count >= 5)
            {
                isFinished = true;

                // Stop skill check loop if running
                if (skillCheckLoop != null)
                    StopCoroutine(skillCheckLoop);

                // Return to main player
                mainPlayer.SetActive(true);
                sittingPlayer.SetActive(false);
            }
            
            // Apply result
            statusManager.SetValue(3, "educational");
            interactAudios[count%2].Play();
            instructionUI?.SetActive(false);
        }
    }
}
