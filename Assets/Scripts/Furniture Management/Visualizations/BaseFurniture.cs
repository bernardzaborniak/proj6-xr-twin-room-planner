using UnityEngine;

public abstract class BaseFurniture : MonoBehaviour
{
    protected FurnitureData localDataCopy;
    [SerializeField] protected MeshFilter boundingBoxMeshFilter;
    [SerializeField] protected MeshRenderer boundingBoxMeshRenderer;
    [SerializeField] protected BoxCollider boxCollider;


    public FurnitureData ConvertToFurnitureDataObject()
    {
        localDataCopy.posInRoom = transform.localPosition;
        localDataCopy.rotInRoom = transform.localRotation;

        return localDataCopy;
    }

    protected void SetBoxCollider()
    {
        boxCollider.center = boundingBoxMeshRenderer.localBounds.center;
        boxCollider.size = boundingBoxMeshRenderer.localBounds.size;
    }
}
