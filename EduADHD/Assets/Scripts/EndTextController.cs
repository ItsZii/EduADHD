using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TextSequenceFader : MonoBehaviour
{
    public TextMeshProUGUI[] texts; // Assign in inspector
    public float fadeDuration = 1f;
    public float holdDuration = 5f;
    public string nextSceneName = "NextSceneName";

    private void Start()
    {
        foreach (var text in texts)
        {
            // Set alpha to 0 before fade in
            SetAlpha(text, 0f);
        }

        StartCoroutine(PlayTextSequence());
    }

    IEnumerator PlayTextSequence()
    {
        foreach (var text in texts)
        {
            // Set alpha to 0 before fade in
            SetAlpha(text, 0f);

            yield return StartCoroutine(FadeText(text, 0f, 1f, fadeDuration)); // Fade in
            yield return new WaitForSeconds(holdDuration);                     // Hold
            yield return StartCoroutine(FadeText(text, 1f, 0f, fadeDuration)); // Fade out
        }

        // After last text fades out, switch scene
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeText(TextMeshProUGUI text, float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            SetAlpha(text, alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        SetAlpha(text, endAlpha); // Final set to ensure exact value
    }

    void SetAlpha(TextMeshProUGUI text, float alpha)
    {
        var color = text.color;
        color.a = alpha;
        text.color = color;
    }
}