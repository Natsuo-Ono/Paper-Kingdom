using TMPro;
using UnityEngine;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Dialogue Settings")]
    public float typingSpeed = 0.03f;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text npcNameText;
    public TMP_Text dialogueText;

    public bool IsDialogueOpen => dialoguePanel.activeSelf;

    // Normal NPC Dialogue
    private string[] currentDialogue;
    private string currentNPCName;

    // NPC Conversation
    private DialogueLine[] currentConversation;

    private bool usingConversation = false;

    private int dialogueIndex;

    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private PlayerMovement player;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        player = FindObjectOfType<PlayerMovement>();
    }

    // ==========================
    // Normal NPC
    // ==========================
    public void StartDialogue(string npcName, string[] dialogue)
    {
        usingConversation = false;

        currentNPCName = npcName;
        currentDialogue = dialogue;
        dialogueIndex = 0;

        npcNameText.text = npcName;

        dialoguePanel.SetActive(true);

        StartTyping();

        player.enabled = false;
    }

    // ==========================
    // NPC Conversation
    // ==========================
    public void StartDialogue(DialogueLine[] dialogue)
    {
        usingConversation = true;

        currentConversation = dialogue;
        dialogueIndex = 0;

        npcNameText.text = currentConversation[0].speakerName;

        dialoguePanel.SetActive(true);

        StartTyping();

        player.enabled = false;
    }

    void Update()
    {
        if (!dialoguePanel.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);

                if (usingConversation)
                    dialogueText.text = currentConversation[dialogueIndex].dialogue;
                else
                    dialogueText.text = currentDialogue[dialogueIndex];

                isTyping = false;
            }
            else
            {
                dialogueIndex++;

                if (usingConversation)
                {
                    if (dialogueIndex >= currentConversation.Length)
                    {
                        EndDialogue();
                    }
                    else
                    {
                        npcNameText.text = currentConversation[dialogueIndex].speakerName;
                        StartTyping();
                    }
                }
                else
                {
                    if (dialogueIndex >= currentDialogue.Length)
                    {
                        EndDialogue();
                    }
                    else
                    {
                        npcNameText.text = currentNPCName;
                        StartTyping();
                    }
                }
            }
        }
    }

    void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (usingConversation)
        {
            typingCoroutine = StartCoroutine(
                TypeSentence(currentConversation[dialogueIndex].dialogue));
        }
        else
        {
            typingCoroutine = StartCoroutine(
                TypeSentence(currentDialogue[dialogueIndex]));
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;

        dialogueText.text = "";

        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        player.enabled = true;
    }
}