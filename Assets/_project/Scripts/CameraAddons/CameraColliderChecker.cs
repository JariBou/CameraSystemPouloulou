using CameraSystem._project.Scripts;
using UnityEngine;

public class CameraColliderChecker : MonoBehaviour
{
    public Transform CamCollider;
    public float speed = 100.0f;
    public float treshHoldDistance = 1.0f;
    public Vector3 test; 
    private CameraConfiguration camConfig = new CameraConfiguration();

    private void Awake()
    {
        if (CamCollider == null)
        {
            Debug.LogError("CamCollider reference is missing! Destroying component.");
            Destroy(this);
        }
    }

    private void Update()
    {
        float distancetoCollider = Vector3.Distance(transform.position, CamCollider.position);

        Vector3 direction = (transform.position - CamCollider.position).normalized;
        RaycastHit hit;

        if(Physics.SphereCast(this.transform.position, 3.0f, direction, out hit, 100))
        {
            test = hit.point;
        }
    }
}   
