using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "Label To Model Conversion Table", menuName = "Room Planner/Label To Model Conversion Table", order = 1)]
public class LabelToModelConversionTable: ScriptableObject
{
    [SerializedDictionary("FurnitureLabel", "Prefab")]
    public SerializedDictionary<FurnitureLabel, GameObject> labelToPrefabDict;

    [Tooltip("Use this if no label was registered in the dict above")]
    public GameObject defaultPrefab;

}