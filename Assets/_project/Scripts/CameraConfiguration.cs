using UnityEngine;

namespace _project.Scripts
{
    [System.Serializable]
    public struct CameraConfiguration
    {
        public float Yaw;
        public float Pitch;
        public float Roll;

        public Vector3 Pivot;
        public float Distance;
        public float Fov;

        public CameraConfiguration(float yaw, float pitch, float roll, Vector3 pivot, float distance, float fov)
        {
            Yaw = yaw;
            Pitch = pitch;
            Roll = roll;
            Pivot = pivot;
            Distance = distance;
            Fov = fov;
        }

        public Quaternion GetRotation()
        {
            return Quaternion.Euler(Pitch, Yaw, Roll);
        }

        public Vector3 GetPosition()
        {
            return Pivot + GetRotation().eulerAngles * Distance; ;
        }

        public void DrawGizmos(Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(Pivot, 0.25f);
            Vector3 position = GetPosition();
            Gizmos.DrawLine(Pivot, position);
            Gizmos.matrix = Matrix4x4.TRS(position, GetRotation(), Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, Fov, 0.5f, 0f, Camera.main.aspect);
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}