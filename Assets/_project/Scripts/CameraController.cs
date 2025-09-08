using _project.Scripts;
using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { 
        get => instance; 
        set 
        {
            if(instance != null)
            {
                Destroy(instance.gameObject);
            }
            instance = value;
        } 
    }
    private static CameraController instance;

    public Camera Camera;
    public CameraConfiguration CameraConfiguration;

    public void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        ApplyConfiguration();
    }
    void ApplyConfiguration()
    {
        Camera.transform.position = CameraConfiguration.GetPosition();
        Camera.transform .rotation = CameraConfiguration.GetRotation();
        Camera.fieldOfView = CameraConfiguration.Fov;
    }

    private void OnDrawGizmos()
    {
        CameraConfiguration.DrawGizmos(Color.red);
    }

}
