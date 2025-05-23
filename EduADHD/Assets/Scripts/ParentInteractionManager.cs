using System.Collections;
using UnityEngine;

public class ParentInteractionManager : MonoBehaviour
{
    [Header("Animation")]
    public Animator modelAnimator;
    public string triggerAnimation = "Talk";
    public string idleAnimation = "Idle";

    [Header("Audio")]
    public AudioSource talkAudio;

    [Header("Trigger Settings")]
    public Collider triggerZone;
    [SerializeField] public StatusManager statusManager;

    private bool hasActivated = false;
    private Quaternion initialRotation;
    private Transform playerTransform;

    void Start()
    {
        initialRotation = transform.rotation;

        if (triggerZone == null)
        {
            Debug.LogWarning("No trigger zone assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasActivated) return;

        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            StartCoroutine(TalkRoutine(other.gameObject));
            hasActivated = true;
        }
    }

    private IEnumerator TalkRoutine(GameObject player)
    {
        //Disable player's movement controller
        CharacterController controller = player.GetComponent<CharacterController>();
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        if (controller != null) controller.enabled = false;
        if (fpc != null) fpc.enabled = false;

        //Rotate MOTHER (this object) to face player
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        lookDirection.y = 0f; // only rotate horizontally
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = lookRotation;

        //Play talking animation
        if (modelAnimator != null)
        {
            modelAnimator.ResetTrigger(idleAnimation);
            modelAnimator.SetTrigger(triggerAnimation);
        }

        //Play audio
        if (talkAudio != null)
        {
            talkAudio.Play();
            yield return new WaitForSeconds(talkAudio.clip.length); // use audio length
        }
        else
        {
            yield return new WaitForSeconds(29f); // fallback duration
        }

        //Rotate MOTHER back to original rotation
        transform.rotation = initialRotation;

        //Lower Player status'
        statusManager.SetValue(-10, "personal");
        statusManager.SetValue(-10, "social");

        //Set back to idle animation
        if (modelAnimator != null)
        {
            modelAnimator.ResetTrigger(triggerAnimation);
            modelAnimator.SetTrigger(idleAnimation);
        }

        //Re-enable player movement
        if (controller != null) controller.enabled = true;
        if (fpc != null) fpc.enabled = true;
    }
}
