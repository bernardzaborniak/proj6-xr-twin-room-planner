using UnityEngine;
using UnityEngine.UI;

public class SelectFurnitureUiMenu : MonoBehaviour
{
    [SerializeField] Button tempButton;
    [SerializeField] GameObject rectEnabledByButton;

    void Start()
    {
        tempButton.onClick.AddListener(ToggleRect);
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
