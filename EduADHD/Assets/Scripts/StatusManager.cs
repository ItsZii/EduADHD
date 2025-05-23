using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider Social;
    [SerializeField] private Slider Education;
    [SerializeField] private Slider Personal;

    [Header("Slider Fills")]
    [SerializeField] private Image SocialFill;
    [SerializeField] private Image EducationFill;
    [SerializeField] private Image PersonalFill;

    [Header("Colors")]
    [SerializeField] private Color low;
    [SerializeField] private Color mid;
    [SerializeField] private Color high;

    void Start()
    {
        Social.value = MainManager.Instance.SocialStatus;
        Education.value = MainManager.Instance.EducationStatus;
        Personal.value = MainManager.Instance.PersonalStatus;
    }

    void Update()
    {
        UpdateSliderColor(Social, SocialFill);
        UpdateSliderColor(Education, EducationFill);
        UpdateSliderColor(Personal, PersonalFill);
        MainManager.Instance.SocialStatus = Social.value;
        MainManager.Instance.EducationStatus = Education.value;
        MainManager.Instance.PersonalStatus = Personal.value;
    }

    public void SetValue(int value, string statusSlider)
    {
        if (statusSlider == "social")
        {
            Social.value += value;
        }
        else if (statusSlider == "educational")
        {
            Education.value += value;
        }
        else if (statusSlider == "personal")
        {
            Personal.value += value;
        }
        else 
        {
            Debug.LogWarning("Could not find passed status type");
        }
    }

    private void UpdateSliderColor(Slider slider, Image fillImage)
    {
        float sliderValue = slider.value;

        if (sliderValue < 40)
        {
            fillImage.color = low;
        }
        else if (sliderValue > 69)
        {
            fillImage.color = high;
        }
        else
        {
            fillImage.color = mid;
        }
    }
}