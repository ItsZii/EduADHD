using UnityEngine;
using System.Collections;

public class ObjectInteraction : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;
    public bool isExit = false;
    [Header("Object Settings")]
    public bool isBed = false;
    public bool isDishes = false;
    public bool isSkillCheck = false;
    public int spoonCost = 0;
    public int socialS = 0;
    public int educationS = 0;
    public int personalS = 0;
    public string taskDesc;
    public string taskDesc2;

    [Header("Location Settings")]
    public bool isHouse = false;
    public bool isSchool = false;
    public bool isMorning = false;
    public bool isEvening = false;

    [Header("References")]
    public AudioSource interactAudio;
    public GameObject instructionUI;
    public BoxCollider triggerZone;

    private bool _isGoingSleep = false;
    public bool IsGoingSleep => _isGoingSleep;

    private bool _isLeavingSchool = false;
    public bool IsLeavingSchool => _isLeavingSchool;

    private bool _isLeavingHome = false;
    public bool IsLeavingHome => _isLeavingHome;

    [Header("External References")]
    [SerializeField] private ClassroomInteraction classroomInteraction;
    [SerializeField] private SkillCheckSlider skillCheckManager;
    [SerializeField] public StatusManager statusManager;
    [SerializeField] private SpoonDisplay SpoonManager;


    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCoroutine;
    private bool _isPlayerInRange = false;
    private bool isActivated = false;
    private Transform _playerTransform;

    private int failedAttempts = 0;
    private const int maxAttempts = 3;
    public bool hasSucceeded = false;
    public bool hasStarted = false;

    void Start()
    {
        _isLeavingSchool = false;
        _isLeavingHome = false;
        _closedRotation = transform.rotation;
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        if (isBed && !isEvening)
        {
            this.GetComponent<MakeBedManager>().setUpBed();
        }

        if (instructionUI) instructionUI.SetActive(false);

        _playerTransform = Camera.main?.transform;

        if (_playerTransform == null)
        {
            Debug.LogWarning("Could not find player transform. Please assign it manually if needed.");
        }
    }

    void Update()
    {
        if (_playerTransform == null || triggerZone == null || hasSucceeded)
            return;

        bool inBounds = triggerZone.bounds.Contains(_playerTransform.position);
        if(isHouse){
            if (isMorning && MainManager.Instance.isMorning)
            {
                if (inBounds && !_isPlayerInRange)
                {
                    _isPlayerInRange = true;
                    instructionUI?.SetActive(true);
                }
                else if (!inBounds && _isPlayerInRange && !GameObject.FindGameObjectWithTag("Player").GetComponent<LeaveHouseManager>().enabled)
                {
                    _isPlayerInRange = false;
                    instructionUI?.SetActive(false);
                }
                if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E) && isExit)
                {
                    if (MainManager.Instance.isPhone && MainManager.Instance.isBag) _isLeavingHome = true;
                    GameObject.FindGameObjectWithTag("Player").GetComponent<LeaveHouseManager>().enabled = true;
                }
            }

            if (isEvening && !MainManager.Instance.isMorning)
            {
                if (_isPlayerInRange && Input.GetKeyDown(KeyCode.F) && isBed)
                {
                    _isGoingSleep = true;
                }
                if (inBounds && !_isPlayerInRange)
                {
                    _isPlayerInRange = true;
                    instructionUI?.SetActive(true);
                }
                else if (!inBounds && _isPlayerInRange)
                {
                    _isPlayerInRange = false;
                    instructionUI?.SetActive(false);
                }
                if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !isSkillCheck && !isBed)
                {
                    this.GetComponent<HomeworkHandler>().enabled = true;
                    instructionUI.SetActive(false);
                    _isPlayerInRange = false;
                    triggerZone.enabled = false;
                    SpoonManager.UpdateSpoonCount(spoonCost);
                    MainManager.Instance.completedTasks.Add(taskDesc2);
                }
                
            }
        }
        

        if(isSchool)
        {
            if (classroomInteraction == null) return;
            if (isSkillCheck)
            {
                if (inBounds && !_isPlayerInRange && isSkillCheck)
                {
                    _isPlayerInRange = true;
                    instructionUI?.SetActive(true);
                }
                else if (!inBounds && _isPlayerInRange && isSkillCheck)
                {
                    _isPlayerInRange = false;
                    instructionUI?.SetActive(false);
                }
            }
            else
            {
                if (inBounds && !_isPlayerInRange && (!classroomInteraction.EndOfLession || isExit))
                {
                    _isPlayerInRange = true;
                    instructionUI?.SetActive(true);
                }

                else if (!inBounds && _isPlayerInRange)
                {
                    _isPlayerInRange = false;
                    instructionUI?.SetActive(false);

                    if (isOpen && !classroomInteraction.EndOfLession)
                    {
                        if (_currentCoroutine != null)
                            StopCoroutine(_currentCoroutine);

                        interactAudio?.Play();
                        _currentCoroutine = StartCoroutine(ToggleDoor());
                    }
                }

                if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !classroomInteraction.EndOfLession && !isExit)
                {
                    if (_currentCoroutine != null)
                        StopCoroutine(_currentCoroutine);

                    interactAudio?.Play();
                    _currentCoroutine = StartCoroutine(ToggleDoor());
                }
                if (classroomInteraction.EndOfLession && !isOpen && !isExit)
                {
                    _currentCoroutine = StartCoroutine(ToggleDoor());
                }
                if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E) && isExit)
                {
                    MainManager.Instance.isMorning = false;
                    _isLeavingSchool = true;
                }
            }
        }
        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !hasStarted && isSkillCheck && failedAttempts < maxAttempts)
        {
            instructionUI.SetActive(false);
            isActivated = true;
            skillCheckManager.Activate(this); // Trigger slider
            _isPlayerInRange = false;
            hasStarted = true;
        }
        

        if (isSkillCheck && isActivated)
        {
            isActivated = false;
            SpoonManager.UpdateSpoonCount(spoonCost);
        }
    }

    private IEnumerator ToggleDoor()
    {
        Quaternion target = isOpen ? _closedRotation : _openRotation;
        isOpen = !isOpen;

        while (Quaternion.Angle(transform.rotation, target) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * openSpeed);
            yield return null;
        }

        transform.rotation = target;
    }

    public void addTaskToList()
    {
        if (MainManager.Instance.isMorning)
        {
            MainManager.Instance.completedTasks.Add(taskDesc);
        }
        else if (!MainManager.Instance.isMorning)
        {
            MainManager.Instance.completedTasks.Add(taskDesc2);
        }
    }

    public void ReportSkillCheckResult(bool success)
    {
        if (success)
        {
            hasSucceeded = true;
            statusManager.SetValue(socialS, "social");
            statusManager.SetValue(educationS, "educational");
            statusManager.SetValue(personalS, "personal");
            interactAudio.Play();
            instructionUI?.SetActive(false);
            addTaskToList();
            if (this.isBed)
            {
                this.GetComponent<MakeBedManager>().MakeBed();
            }
            if (this.isDishes)
            {
                GameObject.FindGameObjectWithTag("dishes").SetActive(false);
            }

        }
        else
        {
            failedAttempts++;

            if (failedAttempts < maxAttempts && _isPlayerInRange)
            {
                instructionUI?.SetActive(true);
                _isPlayerInRange = true;
            }
            else
            {
                instructionUI?.SetActive(false);
                hasSucceeded = true;
            }
        }
        hasStarted = false;
    }

}