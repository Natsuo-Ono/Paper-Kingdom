using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public CharacterInstance character;

    public Image icon;

    public void Setup(CharacterInstance newCharacter)
    {
        character = newCharacter;
        icon.sprite = character.characterData.icon;
    }

    public void SelectCharacter()
    {
        CharacterInfoPanel.Instance.SetCharacter(character);
    }
}