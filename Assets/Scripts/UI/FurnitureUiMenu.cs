using UnityEngine;

public abstract class FurnitureUiMenu : MonoBehaviour
{
    public void OrientToPlayer(Vector3 orientation)
    {
        transform.forward = orientation;
    }
}
