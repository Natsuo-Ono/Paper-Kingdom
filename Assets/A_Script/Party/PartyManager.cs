using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;

    [Header("Starting Characters")]
    public List<Character> startingCharacters = new();

    [Header("Party")]
    public List<CharacterInstance> unlockedCharacters = new();
    public List<CharacterInstance> currentParty = new();

    public const int MaxPartySize = 4;

    void Awake()
    {
        Instance = this;

        unlockedCharacters.Clear();

        foreach (Character character in startingCharacters)
        {
            unlockedCharacters.Add(new CharacterInstance(character));
        }
    }

    public void AddCharacter(CharacterInstance character)
    {
        if (currentParty.Count >= MaxPartySize)
            return;

        if (currentParty.Contains(character))
            return;

        currentParty.Add(character);

        PartyUI.Instance.RefreshParty();
    }

    public void ResetParty()
    {
        currentParty.Clear();

        PartyUI.Instance.RefreshParty();
    }

    public void RemoveCharacter(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= currentParty.Count)
            return;

        currentParty.RemoveAt(slotIndex);

        PartyUI.Instance.RefreshParty();
    }

    public void UnlockCharacter(CharacterInstance character)
    {
        if (!unlockedCharacters.Contains(character))
            unlockedCharacters.Add(character);
    }
}