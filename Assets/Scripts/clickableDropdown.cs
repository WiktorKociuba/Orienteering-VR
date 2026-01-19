using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class clickableDropdown : MonoBehaviour, IPointerClickHandler
{
    public TMP_Dropdown dropdown;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            dropdown.Show();
        }
    }
}
