using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CafeteriaManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource friendMonologue;
    [SerializeField] private AudioSource innerMonologue;
    [SerializeField] private AudioSource praiseAudio;
    [SerializeField] private AudioSource selfHateAudio;

    [Header("UI Elements")]
    [SerializeField] private GameObject concentrationPrompt;
    [SerializeField] private GameObject InstuctionsUI;
    public Slider soundBar;
    public GameObject soundBarUI;

    [Header("Status Manager")]
    [SerializeField] private StatusManager statusManager;
    [SerializeField] private SpoonDisplay SpoonManager;
    
    [Header("Cameras")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera tableCamera;

    [Header("Settings")]
    [SerializeField] private float changeRate = 0.03f;
    [SerializeField] private float keyChange = 0.005f;
    public BoxCollider triggerZone;
    private TextMeshProUGUI concentrationPromptText;
    private float friendVolume;
    private float innerVolume;
    private bool wasAtTable = false;
    public bool isActive = false;
    private bool _isPlayerInRange = false;

    private AudioSource endSound;
    private Transform _playerTransform;
    private Coroutine _currentCoroutine;

void Start()
    {
        concentrationPromptText = concentrationPrompt.GetComponent<TextMeshProUGUI>();
        tableCamera.enabled = false;
        MainManager.Instance.allTasks.Add("Parunāties ar draugiem");
        MainManager.Instance.allTasks.Add("Paēst pusdienas");
        if (concentrationPrompt) concentrationPrompt.SetActive(false);
        if (InstuctionsUI) InstuctionsUI.SetActive(false);

        _playerTransform = Camera.main?.transform;

        if (_playerTransform == null)
        {
            Debug.LogWarning("Could not find player transform. Please assign it manually if needed");
        }
    }
    private void Update()
    {
        if (_playerTransform == null || triggerZone == null)
            return;
        
        if (Input.GetKeyDown(KeyCode.F) && isActive) {
            innerVolume -= keyChange;
            friendVolume += keyChange;
        }
        if (isActive)
        {
            soundBar.value = innerVolume;
        }

        bool inBounds = triggerZone.bounds.Contains(_playerTransform.position);

        if (inBounds && !_isPlayerInRange && !isActive && !wasAtTable)
        {
            _isPlayerInRange = true;
            InstuctionsUI?.SetActive(true);
        }
        else if (!inBounds && _isPlayerInRange)
        {
            _isPlayerInRange = false;
            InstuctionsUI?.SetActive(false);
        }

        if (_isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !wasAtTable)
        {
            SpoonManager.UpdateSpoonCount(3); 

            mainCamera.enabled = false;
            tableCamera.enabled = true;
            mainCamera.GetComponent<AudioListener>().enabled = false;
            tableCamera.GetComponent<AudioListener>().enabled = true;

            InstuctionsUI?.SetActive(false);
            soundBarUI.SetActive(true);

            _currentCoroutine = StartCoroutine(Talking());
        }
    }

    private IEnumerator Talking()
    {
        if (!isActive)
        {
            StartConversation();
        }

        while (isActive)
        {
            yield return new WaitForSeconds(1);
            HandleVolumeChange();
            ApplyVolumes();
            CheckEndConditions();
        }
        yield return new WaitForSeconds(2);
        endSound.Play();
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = false;
        yield return new WaitForSeconds(endSound.clip.length);
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
    }

    public void StartConversation()
    {
        wasAtTable = true;
        isActive = true;
        friendVolume = 0.95f;
        innerVolume = 0.05f;
        friendMonologue.Play();
        innerMonologue.Play();
        MainManager.Instance.completedTasks.Add("Parunāties ar draugiem");
        mainCamera.GetComponent<AudioListener>().enabled = false;
        tableCamera.GetComponent<AudioListener>().enabled = true;
    }

    private void HandleVolumeChange()
    {
        // Every second, inner gets louder, friend gets quieter
        friendVolume -= changeRate;
        innerVolume += changeRate;

        if (innerVolume >= 0.87f)
        {
            changeTextProperties(72, "Tu esi neveiksme, neviens Tevi te tāpat negrib redzēt");
        }
        else if (innerVolume >= 0.72f)
        {
            changeTextProperties(64, "Kādēļ Tu vispār mēģini...\n\nTev vajadzētu padoties un iet mājās");
        }
        else if (innerVolume >= 0.6f)
        {
            changeTextProperties(56, "Tapēc ar Tevi neviens nedraudzējās, Tu nekad neklausies\n\nSpied taču [F]!!!");
        }
        else if (innerVolume >= 0.45f)
        {
            changeTextProperties(42, "Spaidi [F], lai pievērstu uzmanību sarunai\n\nVarbūt klausies ko Tev stāsta?");
        }
        else if (innerVolume >= 0.3f)
        {
            concentrationPrompt.SetActive(true);
        }
    }

    private void ApplyVolumes()
    {
        friendMonologue.volume = friendVolume;
        innerMonologue.volume = innerVolume;
    }

    private void CheckEndConditions()
    {
        if (innerVolume >= 0.98f)
        {
            EndConversation(false);
        }
        else if (friendVolume >= 0.98f)
        {
            EndConversation(true);
        }
    }

    private void EndConversation(bool success)
    {
        isActive = false;
        friendMonologue.Stop();
        innerMonologue.Stop();
        concentrationPrompt.SetActive(false);
        tableCamera.enabled = false;
        mainCamera.enabled = true;
        mainCamera.GetComponent<AudioListener>().enabled = true;
        tableCamera.GetComponent<AudioListener>().enabled = false;
        soundBarUI.SetActive(false);

        if (success)
        {
            endSound = praiseAudio;
            statusManager.SetValue(10, "social");
            statusManager.SetValue(5, "personal");
        }
        else
        {
            endSound = selfHateAudio;
            statusManager.SetValue(-15, "social");
            statusManager.SetValue(-10, "personal");
        }
        mainCamera.GetComponent<AudioListener>().enabled = true;
        tableCamera.GetComponent<AudioListener>().enabled = false;
    }
    void changeTextProperties(int size, string newText)
    {
        concentrationPromptText.fontSize = size;

        if (newText != "")
        {
            concentrationPromptText.SetText(newText);
        } 
    }
}
