using UnityEngine;

public interface IInteractableFurniture 
{

    public bool Interactable { get; set; }

    public void OnHoverStart();

    public void OnHoverEnd();

    public void OnSelect(Vector3 selectDirection);

    public void OnDeselect();
}
