using UnityEngine;
using UnityEngine.EventSystems;

public class PartySlot : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            PartyManager.Instance.RemoveCharacter(slotIndex);
        }
    }
}