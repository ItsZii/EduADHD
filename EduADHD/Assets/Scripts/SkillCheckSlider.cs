using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillCheckSlider : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider skillSlider;
    public RectTransform arrowImage;
    public GameObject instructionUI_bottom;

    [Header("Rotation Settings")]
    public float moveSpeed = 1.0f;
    private bool movingForward = true;
    private Coroutine moveCoroutine;
    private bool isActive = false;
    private Vector3 originalArrowPos;
    public float bobSpeed = 2f;
    public float bobHeight = 10f;

    [Header("Audio")]
    public AudioSource fail;

    private ObjectInteraction currentTarget;
    private HomeworkHandler currentTarget2;
    private ClassroomInteraction classroomTarget;

    private void OnEnable()
    {
        isActive = false;

        DisablePlayerControl();
        movingForward = true;

        if (skillSlider != null)
        {
            skillSlider.value = 0f;
            skillSlider.gameObject.SetActive(false);
        }

        instructionUI_bottom?.SetActive(false);

        if (arrowImage != null)
            originalArrowPos = arrowImage.anchoredPosition;

        StartSkillCheck();
    }

    private void OnDisable()
    {
        EnablePlayerControl();

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        skillSlider?.gameObject.SetActive(false);
        instructionUI_bottom?.SetActive(false);
    }

    private void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.F))
        {
            EndSkillCheck();
        }
    }

    void StartSkillCheck()
    {
        isActive = true;

        instructionUI_bottom?.SetActive(true);
        skillSlider.value = 0f;
        skillSlider.gameObject.SetActive(true);

        moveCoroutine = StartCoroutine(MoveHandle());
    }

    void EndSkillCheck()
    {
        isActive = false;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        float result = skillSlider.value;
        bool successResult = result > 0.40f && result < 0.61f;

        if (!successResult) fail?.Play();

        skillSlider.gameObject.SetActive(false);
        instructionUI_bottom?.SetActive(false);

        if (currentTarget != null)
        {
            currentTarget.ReportSkillCheckResult(successResult);
            currentTarget = null;
        }
        else if (currentTarget2 != null)
        {
            currentTarget2.ReportSkillCheckResult(successResult);
            currentTarget2 = null;
        }
        else if (classroomTarget != null)
        {
            classroomTarget.ReportSkillCheckResult(successResult);
            classroomTarget = null;
        }

        gameObject.SetActive(false);
    }

    IEnumerator MoveHandle()
    {
        while (true)
        {
            float direction = movingForward ? 1f : -1f;
            skillSlider.value += direction * moveSpeed * Time.unscaledDeltaTime;

            if (skillSlider.value >= 1f)
            {
                skillSlider.value = 1f;
                movingForward = false;
            }
            else if (skillSlider.value <= 0f)
            {
                skillSlider.value = 0f;
                movingForward = true;
            }

            if (arrowImage != null)
            {
                float bobOffset = Mathf.Sin(Time.unscaledTime * bobSpeed) * bobHeight;
                arrowImage.anchoredPosition = originalArrowPos + new Vector3(0f, bobOffset, 0f);
            }

            yield return null;
        }
    }

    public void Activate(ObjectInteraction target)
    {
        currentTarget = target;
        gameObject.SetActive(true);
    }

    public void ActivateHW(HomeworkHandler target)
    {
        currentTarget2 = target;
        gameObject.SetActive(true);
    }

    public void ActivateClassroom(ClassroomInteraction target)
    {
        classroomTarget = target;
        gameObject.SetActive(true);
    }

    private void DisablePlayerControl()
    {
        var p1 = GameObject.FindGameObjectWithTag("Player")?.GetComponent<FirstPersonController>();
        var p2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<FirstPersonController>();
        if (p1) p1.enabled = false;
        if (p2) p2.enabled = false;
    }

    private void EnablePlayerControl()
    {
        var p1 = GameObject.FindGameObjectWithTag("Player")?.GetComponent<FirstPersonController>();
        var p2 = GameObject.FindGameObjectWithTag("Player2")?.GetComponent<FirstPersonController>();
        if (p1) p1.enabled = true;
        if (p2) p2.enabled = true;
    }
}
