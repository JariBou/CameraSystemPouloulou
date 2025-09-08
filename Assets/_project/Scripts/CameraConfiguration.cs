using UnityEngine;
using UnityEngine.Serialization;

namespace CameraSystem._project.Scripts
{
    [System.Serializable]
    public struct CameraConfiguration
    {
        [FormerlySerializedAs("Yaw")] public float yaw;
        [FormerlySerializedAs("Pitch")] public float pitch;
        [FormerlySerializedAs("Roll")] public float roll;

        [FormerlySerializedAs("Pivot")] public Vector3 pivot;
        [FormerlySerializedAs("Distance")] public float distance;
        [FormerlySerializedAs("Fov")] public float fov;

        public CameraConfiguration(float yaw, float pitch, float roll, Vector3 pivot, float distance, float fov)
        {
            this.yaw = yaw;
            this.pitch = pitch;
            this.roll = roll;
            this.pivot = pivot;
            this.distance = distance;
            this.fov = fov;
        }

        public Quaternion GetRotation()
        {
            return Quaternion.Euler(pitch, yaw, roll);
        }

        public Vector3 GetPosition()
        {
            return pivot + GetRotation() * (Vector3.back * distance);
        }

        public void DrawGizmos(Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(pivot, 0.25f);
            Vector3 position = GetPosition();
            Gizmos.DrawLine(pivot, position);
            Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, fov, 0.5f, 0f, Camera.main.aspect);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}