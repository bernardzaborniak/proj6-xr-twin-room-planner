using UnityEngine;

[System.Serializable]
public class SpawnMenuFurnitureItem
{
    public string fName;
    //public FurnitureData data;
    public FurnitureLabel label;

    [Tooltip("use this to adjust the size of spawned objects")]
    public Vector3 boundsSizeAdjustment = new Vector3(1,1,1);
    //public GameObject meshToInitializeToCopySizeFrom;
    public Sprite image;
}
