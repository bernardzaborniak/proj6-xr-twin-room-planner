using UnityEngine;

public class WallPlacedByPlayer : MonoBehaviour
{
    // empty just sed to find objects to delete
    [SerializeField] Material scanMaterial;
    [SerializeField] Material layoutMaterial;
    [SerializeField] MeshRenderer myRenderer;

     void Start()
    {
        RoomsManager.Instance.OnChangeToLayoutMode += ChangeToLayoutLook;
        RoomsManager.Instance.OnChangeToScanMode += ChangeToScanLook;
    }

      void OnDestroy()
    {
        RoomsManager.Instance.OnChangeToLayoutMode -= ChangeToLayoutLook;
        RoomsManager.Instance.OnChangeToScanMode -= ChangeToScanLook;
    }

    void ChangeToLayoutLook()
    {
        myRenderer.material = layoutMaterial;
    }

    void ChangeToScanLook()
    {
        myRenderer.material = scanMaterial;

    }
}
