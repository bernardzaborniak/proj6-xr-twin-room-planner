using UnityEngine;
using UnityEngine.UI;

public class SelectFurnitureUiMenu : MonoBehaviour
{
    [SerializeField] UiInteractionCustomButton tempButton;
    [SerializeField] GameObject rectEnabledByButton;

    void Start()
    {
        tempButton.OnClick += ToggleRect;
    }

    public void OrientToPlayer(Vector3 orientation)
    {
        transform.forward = orientation;
    }

    void ToggleRect()
    {
        rectEnabledByButton.SetActive(!rectEnabledByButton.activeSelf);
    }

}
