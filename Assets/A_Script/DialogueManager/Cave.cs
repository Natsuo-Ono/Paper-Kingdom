using System.Collections;
using UnityEngine;
using TMPro;

public class CaveTeleport : MonoBehaviour
{
    [Header("Teleport")]
    public Transform teleportDestination;

    [TextArea]
    public string interactionMessage = "[E] Enter Cave";

    [Header("UI")]
    public GameObject interactionPrompt;
    public TMPro.TMP_Text interactionText;

    [Header("Fade")]
    public CanvasGroup fadeScreen;
    public float fadeDuration = 0.75f;

    private bool playerNearby;
    private PlayerMovement player;

    void Start()
    {
        interactionPrompt.SetActive(false);

        player = FindObjectOfType<PlayerMovement>();

        fadeScreen.alpha = 0f;
        fadeScreen.blocksRaycasts = false;
    }

    void Update()
    {
        if (!playerNearby)
            return;

        if (DialogueManager.Instance.IsDialogueOpen)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TeleportTransition());
        }
    }

    private IEnumerator TeleportTransition()
    {
        interactionPrompt.SetActive(false);

        playerNearby = false;

        // Fade In
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeScreen.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        fadeScreen.alpha = 1f;

        // Teleport when screen is fully black
        player.transform.position = teleportDestination.position;

        yield return new WaitForSeconds(0.2f);

        // Fade Out
        t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeScreen.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        fadeScreen.alpha = 0f;
    }

    private void OnDrawGizmos()
    {
        if (teleportDestination == null)
            return;

        Gizmos.color = Color.green;

        // Draw a sphere at the destination
        Gizmos.DrawSphere(teleportDestination.position, 0.3f);

        // Draw a line from the cave to the destination
        Gizmos.DrawLine(transform.position, teleportDestination.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = true;

        interactionPrompt.SetActive(true);

        interactionText.text = interactionMessage;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = false;

        interactionPrompt.SetActive(false);
    }
}