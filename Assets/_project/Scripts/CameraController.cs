using System;
using System.Collections.Generic;
using CameraSystem._project.Scripts.Extensions;
using CameraSystem._project.Scripts.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraSystem._project.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { 
            get => instance;
            private set 
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
        private CameraConfiguration _targetCameraConfiguration;
        [SerializeField]
        private List<ViewBase> _activeViews = new();

        [SerializeField, Range(0.01f, 1000f)]
        private float _speed = 0.1f;

        public void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _currentCameraConfiguration = new CameraConfiguration(0,
                0,
                0,
                camera.transform.position,
                0,
                camera.fieldOfView);
            _currentCameraConfiguration.UpdateRotation(camera.transform.rotation);
        }

        private void Update()
        {
            _targetCameraConfiguration = ComputeAverage();
            ApplyConfiguration();
        }
        
        void ApplyConfiguration()
        {
            // _currentCameraConfiguration = CameraConfiguration.Lerp(_currentCameraConfiguration, _targetCameraConfiguration, Time.deltaTime * _speed);
            _currentCameraConfiguration.LerpTo(_targetCameraConfiguration, Time.deltaTime * _speed);
            
            camera.transform.position = _currentCameraConfiguration.GetPosition();
            camera.transform.rotation = _currentCameraConfiguration.GetRotation();
            camera.fieldOfView = _currentCameraConfiguration.fov;

            // camera.transform.position = Vector3.Lerp(_currentCameraConfiguration.GetPosition(), _targetCameraConfiguration.GetPosition(), Time.deltaTime * _speed);
            // camera.transform.rotation = Quaternion.Lerp(_currentCameraConfiguration.GetRotation(), _targetCameraConfiguration.GetRotation(), Time.deltaTime * _speed);
            // camera.fieldOfView = Mathf.Lerp(_currentCameraConfiguration.fov, _targetCameraConfiguration.fov,  Time.deltaTime * _speed);
            //
            // _currentCameraConfiguration.UpdateRotation(camera.transform.rotation);
            // _currentCameraConfiguration.pivot = Vector3.Lerp(_currentCameraConfiguration.pivot, _targetCameraConfiguration.pivot, Time.deltaTime * _speed);
            // _currentCameraConfiguration.fov = camera.fieldOfView;
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
