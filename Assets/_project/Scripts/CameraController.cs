using System.Collections.Generic;
using CameraSystem._project.Scripts.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraSystem._project.Scripts
{
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

        [FormerlySerializedAs("Camera")] public Camera camera;
        
        private CameraConfiguration _currentCameraConfiguration;
        [SerializeField]
        private List<ViewBase> _activeViews = new();

        public void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            _currentCameraConfiguration = ComputeAverage();
            ApplyConfiguration();
        }
        
        void ApplyConfiguration()
        {
            camera.transform.position = _currentCameraConfiguration.GetPosition();
            camera.transform .rotation = _currentCameraConfiguration.GetRotation();
            camera.fieldOfView = _currentCameraConfiguration.fov;
        }

        private CameraConfiguration ComputeAverage()
        {
            float pitchSum = 0f;
            float rollSum = 0f;
            float distanceSum = 0f;
            float fovSum = 0f;
            Vector3 pivotSum  = Vector3.zero;
            float weightSum = 0f;
            foreach (ViewBase view in _activeViews)
            {
                float viewWeight = view.weight;
                weightSum += viewWeight;
                CameraConfiguration viewConfiguration = view.GetConfiguration();
                pitchSum += viewConfiguration.pitch * viewWeight;
                rollSum += viewConfiguration.roll * viewWeight;
                distanceSum += viewConfiguration.distance * viewWeight;
                fovSum += viewConfiguration.fov * viewWeight;
                pivotSum += viewConfiguration.pivot * viewWeight;
            }

            if (weightSum == 0f)
            {
                weightSum = 1;
            }

            return new CameraConfiguration(ComputeAverageYaw(), pitchSum/weightSum, rollSum/weightSum, pivotSum/weightSum, distanceSum/weightSum, fovSum/weightSum);
        }
        
        private float ComputeAverageYaw()
        {
            Vector2 sum = Vector2.zero;
            foreach (ViewBase view in _activeViews)
            {
                CameraConfiguration config = view.GetConfiguration();
                sum += new Vector2(Mathf.Cos(config.yaw * Mathf.Deg2Rad),
                    Mathf.Sin(config.yaw * Mathf.Deg2Rad)) * view.weight;
            }
            return Vector2.SignedAngle(Vector2.right, sum);
        }


        public void AddView(ViewBase view)
        {
            ViewBase viewBase = _activeViews.Find(@base => @base == view);
            if (viewBase is null)
            {
                _activeViews.Add(view);
            }
        }

        public void RemoveView(ViewBase view)
        {
            _activeViews.Remove(view);
        }

        private void OnDrawGizmos()
        {
            _currentCameraConfiguration.DrawGizmos(Color.red);
        }

    }
}
