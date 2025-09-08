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

        Quaternion GetRotation()
        {
            return Quaternion.Euler(Pitch, Yaw, Roll);
        }

        Vector3 GetPosition()
        {
            return Pivot + GetRotation().eulerAngles * Distance; ;
        }
    }
}