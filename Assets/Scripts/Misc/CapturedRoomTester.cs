using Meta.XR.MRUtilityKit;
using UnityEngine;

public class CapturedRoomTester : MonoBehaviour
{
    [SerializeField] MRUKRoom mrukRoom;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            foreach (MRUKAnchor anchor  in mrukRoom.Anchors)
            {
                Debug.Log($"[MRUK anchors name: {anchor.name}  label: {anchor.Label} ");
            }
        }
    }
}
