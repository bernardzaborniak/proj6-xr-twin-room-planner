using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiInteractionHelper : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TMP_Dropdown dropdown;

    public void OnClick()
    {
        button?.onClick.Invoke();

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = transform.position
        };

        dropdown?.OnPointerClick(pointerEventData);
    }
}
