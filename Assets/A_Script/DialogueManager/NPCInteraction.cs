using TMPro;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC")]
    public string interactName;

    public DialogueLine[] dialogue;

    [Header("UI")]
    public GameObject interactionPrompt;
    public TMP_Text interactionText;

    private bool playerNearby;

    void Start()
    {
        interactionPrompt.SetActive(false);
    }

    void Update()
    {
        if (!playerNearby)
            return;

        if (DialogueManager.Instance.IsDialogueOpen)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            DialogueManager.Instance.StartDialogue(dialogue);

            interactionPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = true;

        interactionPrompt.SetActive(true);

        interactionText.text = $"[E] {interactName}";
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = false;

        interactionPrompt.SetActive(false);
    }
}