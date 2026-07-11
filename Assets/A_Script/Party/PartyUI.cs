using UnityEngine;
using UnityEngine.UI;

public class PartyUI : MonoBehaviour
{
    public static PartyUI Instance;

    [Header("UI")]
    public GameObject partyPanel;

    [Header("Character Grid")]
    public CharacterButton characterButtonPrefab;
    public Transform gridParent;

    public Image[] partySlots;

    private PlayerMovement player;

    public bool IsPartyOpen => partyPanel.activeSelf;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();

        if (partyPanel != null)
            partyPanel.SetActive(false);

        foreach (CharacterInstance character in PartyManager.Instance.unlockedCharacters)
        {
            Debug.Log("Spawning: " + character.characterData.characterName);

            CharacterButton button =
                Instantiate(characterButtonPrefab, gridParent);

            button.Setup(character);
        }

        RefreshParty();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleParty();
        }
    }

    public void ToggleParty()
    {
        if (partyPanel == null)
            return;

        bool open = !partyPanel.activeSelf;

        partyPanel.SetActive(open);

        if (player != null)
            player.enabled = !open;
    }

    public void OpenParty()
    {
        if (partyPanel == null)
            return;

        partyPanel.SetActive(true);

        if (player != null)
            player.enabled = false;
    }

    public void CloseParty()
    {
        if (partyPanel == null)
            return;

        partyPanel.SetActive(false);

        if (player != null)
            player.enabled = true;
    }

    public void RefreshParty()
    {
        for (int i = 0; i < partySlots.Length; i++)
        {
            if (i < PartyManager.Instance.currentParty.Count)
            {
                partySlots[i].sprite = PartyManager.Instance.currentParty[i].characterData.icon;

                partySlots[i].enabled = true;
            }
            else
            {
                partySlots[i].enabled = false;
            }
        }
    }
}