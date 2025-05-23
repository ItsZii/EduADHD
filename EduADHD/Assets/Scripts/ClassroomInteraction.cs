using UnityEngine;
using System.Collections;

public class ClassroomInteraction : MonoBehaviour
{
    [Header("References")]
    public AudioSource chairAudio;
    public AudioSource thoughtAudio;
    public AudioSource teacherAudio;
    public AudioSource bellAudio;
    public GameObject sitInstructionUI;
    public GameObject askInstructionUI;
    public GameObject writeInstructionUI;
    public GameObject paperObj;
    public BoxCollider triggerZone;
    public AudioSource hallwaySound;
    public AudioSource cafeteriaSound;

    [SerializeField] private Camera deskCamera;    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject deskPlayer;    
    [SerializeField] private GameObject mainPlayer;
    [SerializeField] private Transform teleportLocation;

    [Header("Skill Check Settings")]
    [SerializeField] private SkillCheckSlider skillCheckManager;
    [SerializeField] private float lessonDuration = 100f;

    [Header("External References")]
    [SerializeField] private SpoonDisplay SpoonManager;
    [SerializeField] public StatusManager statusManager;
    [SerializeField] public ClassroomCollision classCollision;

    private bool _isPlayerInRange = false;
    private bool hasPaper = false;
    private bool _playerIsSeated = false;
    private bool _endOfLession = false;
    private bool introDone;

    private Transform _playerTransform;
    private Camera currCam;
    private float remainingLessonTime;
    private bool skillCheckInProgress = false;
    private Coroutine lessonTimerCoroutine;

    public bool PlayerIsSeated => _playerIsSeated;
    public bool EndOfLession => _endOfLession;

    void Start()
    {
        MainManager.Instance.allTasks.Add("Piedalīties stundā");
        MainManager.Instance.allTasks.Add("Veikt pierakstus stundā");

        currCam = Camera.main;
        _playerTransform = Camera.main?.transform;
        remainingLessonTime = lessonDuration;

        paperObj.SetActive(false);
        introDone = false;

        deskCamera.enabled = false;
        mainCamera.enabled = true;
        mainCamera.GetComponent<AudioListener>().enabled = true;
        deskCamera.GetComponent<AudioListener>().enabled = false;

        deskPlayer.SetActive(false);
        mainPlayer.SetActive(true);

        sitInstructionUI?.SetActive(false);
        askInstructionUI?.SetActive(false);
        writeInstructionUI?.SetActive(false);
    }

    void Update()
    {
        if (_playerTransform == null || triggerZone == null)
            return;

        bool inBounds = triggerZone.bounds.Contains(_playerTransform.position);

        if (inBounds && !_isPlayerInRange && !_playerIsSeated && !_endOfLession)
        {
            _isPlayerInRange = true;
            sitInstructionUI?.SetActive(true);
        }
        else if (!inBounds && _isPlayerInRange)
        {
            _isPlayerInRange = false;
            sitInstructionUI?.SetActive(false);
        }

        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !_endOfLession)
        {
            SpoonManager.UpdateSpoonCount(3); 
            chairAudio?.Play();
            MainManager.Instance.completedTasks.Add("Piedalīties stundā");

            mainCamera.enabled = false;
            deskCamera.enabled = true;
            mainCamera.GetComponent<AudioListener>().enabled = false;
            deskCamera.GetComponent<AudioListener>().enabled = true;
            deskPlayer.SetActive(true);

            _playerIsSeated = true;
            sitInstructionUI?.SetActive(false);

            StartCoroutine(Lession());
        }

        if (classCollision.PlayerInBoyView && _playerIsSeated && !hasPaper && !_endOfLession && introDone)
        {
            askInstructionUI?.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                askInstructionUI?.SetActive(false);
                hasPaper = true;
                SpoonManager.UpdateSpoonCount(2);
                paperObj.SetActive(true);
                statusManager.SetValue(5, "social");
                MainManager.Instance.completedTasks.Add("Veikt pierakstus stundā");
            }
        }
        else if (!classCollision.PlayerInBoyView && _playerIsSeated)
        {
            askInstructionUI?.SetActive(false);
        }
    }

    private IEnumerator Lession()
    {
        statusManager.SetValue(10, "educational");
        yield return new WaitForSeconds(10);
        thoughtAudio?.Play();
        yield return new WaitForSeconds(22);
        introDone = true;

        lessonTimerCoroutine = StartCoroutine(LessonTimerWithSkillChecks());

        while (remainingLessonTime > 0f)
            yield return null;

        EndLesson();
    }

    private IEnumerator LessonTimerWithSkillChecks()
    {
        while (remainingLessonTime > 0f)
        {
            float waitTime = Random.Range(4f, 10f);
            yield return new WaitForSeconds(waitTime);

            if (_endOfLession) yield break;

            skillCheckInProgress = true;
            Time.timeScale = 0f;
            skillCheckManager.ActivateClassroom(this);
            yield return new WaitUntil(() => skillCheckInProgress == false);
            Time.timeScale = 1f;

            remainingLessonTime -= waitTime;
        }
    }

    private void EndLesson()
    {
        teacherAudio?.Stop();
        bellAudio?.Play();
        _playerIsSeated = false;
        _endOfLession = true;

        deskCamera.enabled = false;
        mainCamera.enabled = true;
        mainCamera.GetComponent<AudioListener>().enabled = true;
        deskCamera.GetComponent<AudioListener>().enabled = false;

        deskPlayer.SetActive(false);
        askInstructionUI?.SetActive(false);
        writeInstructionUI?.SetActive(false);
        hallwaySound.Play();
        cafeteriaSound.Play();

        if (teleportLocation != null)
        {
            mainPlayer.transform.position = teleportLocation.position;
            mainPlayer.transform.rotation = teleportLocation.rotation;
        }
    }

    public void ReportSkillCheckResult(bool success)
    {
        float value = hasPaper ? 5f : 3f;
        int result = success ? (int)value : -(int)value;
        statusManager.SetValue(result, "educational");
        skillCheckInProgress = false;
    }
}