using UnityEngine;

public class Gun : MonoBehaviour
{
    public OVRInput.RawButton shootingButton;

    public Transform shootPoint;
    public GameObject shootRayPrefab;
    //public LineRenderer shootRay;
    public float maxLineDistance;
    public LayerMask shootLayerMask;

    public float destroyTimeout = 5f;

    public AudioSource shootSoundSource;
    public AudioClip shootSoundClip;
    public GameObject rayImpactPrefab;


    void Start()
    {
        
    }

    void Update()
    {
        if (OVRInput.GetDown(shootingButton)) 
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Vector3 lineEnd = shootPoint.position + shootPoint.forward * maxLineDistance;

        Ray ray = new Ray(shootPoint.position, shootPoint.forward); 
        if(Physics.Raycast(ray, out RaycastHit hit, maxLineDistance, shootLayerMask))
        {
            lineEnd = hit.point;

            GameObject impact = Instantiate(rayImpactPrefab, hit.point, Quaternion.LookRotation(-hit.normal));
            Destroy(impact.gameObject, destroyTimeout);
        }

        LineRenderer shootRay = Instantiate(shootRayPrefab).GetComponent<LineRenderer>();
        shootRay.positionCount = 2;
        shootRay.SetPosition(0, shootPoint.position);
        shootRay.SetPosition(1, lineEnd);

        Destroy(shootRay.gameObject, destroyTimeout);

        Debug.Log("Shooting");


        shootSoundSource.PlayOneShot(shootSoundClip);
    }
}
