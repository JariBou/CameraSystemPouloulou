using CameraSystem._project.Scripts;
using CameraSystem._project.Scripts.Views;
using UnityEngine;

namespace CameraSystem
{
    public class DollyView : ViewBase
    {
        public float roll;
        public float fov;
        public float distance;
        public GameObject target;
        public Rail rail;
        public float distanceOnRail;
        public float speed;
        public bool IsAuto;

        private Vector2 YawPitch;
        private void Update()
        {
            YawPitch = CalculateYawPitchToTarget(target.transform.position);
            float axisX = Input.GetAxis("Horizontal");
            distanceOnRail += (axisX * Time.deltaTime) * speed;

            transform.position = rail.GetPosition(distanceOnRail);
        }

        public Vector2 CalculateYawPitchToTarget(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - this.transform.position;

            float yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float horizontalDistance = Mathf.Sqrt(direction.x * direction.x + direction.z * direction.z);
            float pitch = Mathf.Atan2(direction.y, horizontalDistance) * Mathf.Rad2Deg;

            return new Vector2(yaw, pitch);
        }

        public override CameraConfiguration GetConfiguration()
        {
            return new CameraConfiguration(YawPitch.x, YawPitch.y, 0, transform.position, 0, fov);
        }
    }
}
