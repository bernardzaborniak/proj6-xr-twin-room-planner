using Meta.XR.MRUtilityKit;
using UnityEngine;

public class CapturedRoomTester : MonoBehaviour
{
    [SerializeField] MRUKRoom mrukRoom;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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
