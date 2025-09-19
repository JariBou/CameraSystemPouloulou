using System;
using System.Collections;
using System.Collections.Generic;
using CameraSystem._project.Scripts.Extensions;
using CameraSystem._project.Scripts.Views;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraSystem._project.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance
        {
            get => instance;
            private set
            {
                if (instance != null)
                {
                    Destroy(instance.gameObject);
                }
                instance = value;
            }
        }

        public List<ViewBase> ActiveViews { get => _activeViews; set => _activeViews = value; }

        private static CameraController instance;

        [FormerlySerializedAs("Camera")] public Camera camera;

        private CameraConfiguration _currentCameraConfiguration;
        private CameraConfiguration _targetCameraConfiguration;
        private CameraConfiguration _finalCameraConfiguration;
        [SerializeField]
        private List<ViewBase> _activeViews = new();

        [SerializeField, Range(0.01f, 1000f)]
        private float _speed = 0.1f;

        private bool _isCutRequested;

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
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (shakeCoroutine != null)
                    StopCoroutine(shakeCoroutine);
                shakeCoroutine = StartCoroutine(ShakeCoroutine());
            }

            _targetCameraConfiguration = ComputeAverage();
            if (_isCutRequested)
            {
                _currentCameraConfiguration = _targetCameraConfiguration;
                _isCutRequested = false;
            }
            ApplyConfiguration();
        }


        public float shakeDuration = 2.0f;
        public float noise = 5.0f;
        public float shakeSpeed = 5.0f;
        public float shakeXmax = 10.0f;
        public float shakeYmax = 10.0f;
        public float shakeZmax = 10.0f;

        private Coroutine shakeCoroutine;
        private IEnumerator ShakeCoroutine()
        {
            Vector3 savedCamPosition = camera.transform.position;
            float shakeDurationCopy = shakeDuration;
            float intensity = 1.0f;
            float randomSeed = UnityEngine.Random.Range(0f, 100f);

            float offsetX;
            float offsetY;
            float offsetZ;
            float t = 0;
            float speed = 1.0f / shakeDuration;

            Debug.Log("started shake coroutine");
            while (shakeDurationCopy > 0)
            {
                shakeDurationCopy -= Time.deltaTime;

                intensity = Mathf.Max(intensity - Time.deltaTime * speed, 0);
                t += Time.deltaTime * shakeSpeed;
                float noiseX = Mathf.PerlinNoise(randomSeed, t) - 0.5f;
                float noiseY = Mathf.PerlinNoise(randomSeed + 33.33f, t) - 0.5f;
                float noiseZ = Mathf.PerlinNoise(randomSeed + 1824.22f, t) - 0.5f;
                offsetX = noiseX * shakeXmax * intensity * intensity;
                offsetY = noiseY * shakeYmax * intensity * intensity;
                offsetZ = noiseZ * shakeZmax * intensity * intensity;


                _finalCameraConfiguration = _currentCameraConfiguration;
                _finalCameraConfiguration.pitch += offsetX;
                _finalCameraConfiguration.yaw += offsetY;
                _finalCameraConfiguration.roll += offsetZ;

                yield return null;
            }
            _finalCameraConfiguration = _currentCameraConfiguration;
            Debug.Log("ended shake coroutine");
            shakeCoroutine = null;
        }

        public void Cut()
        {
            _isCutRequested = true;
        }

        void ApplyConfiguration()
        {
            _currentCameraConfiguration.LerpTo(_targetCameraConfiguration, Time.deltaTime * _speed);

            camera.transform.position = _currentCameraConfiguration.GetPosition();
            camera.transform.rotation = _currentCameraConfiguration.GetRotation();
            camera.fieldOfView = _currentCameraConfiguration.fov;

            if (shakeCoroutine == null)
            {
                _finalCameraConfiguration = _currentCameraConfiguration;
            }

            camera.transform.position = _finalCameraConfiguration.GetPosition();
            camera.transform.rotation = _finalCameraConfiguration.GetRotation();
            camera.fieldOfView = _finalCameraConfiguration.fov;
        }

        private CameraConfiguration ComputeAverage()
        {
            float pitchSum = 0f;
            float rollSum = 0f;
            float distanceSum = 0f;
            float fovSum = 0f;
            Vector3 pivotSum = Vector3.zero;
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
                sum += new Vector2(
                    Mathf.Cos(config.yaw * Mathf.Deg2Rad),
                    Mathf.Sin(config.yaw * Mathf.Deg2Rad)
                    ) * view.weight;
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
