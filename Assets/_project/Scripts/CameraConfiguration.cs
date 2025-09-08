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

        public void UpdateRotation(Quaternion transformRotation)
        {
            yaw = transformRotation.eulerAngles.y;
            pitch = transformRotation.eulerAngles.x;
            roll = transformRotation.eulerAngles.z;
        }

        /// <summary>
        /// This method is <see href="https://media.tenor.com/CEO9iZ_AmygAAAAe/jeff-the-land-shark-absolute-cinema.png">this</see>
        /// </summary>
        /// <param name="a"> a </param>
        /// <param name="b"> b </param>
        /// <param name="t"> The t of the lerp </param>
        public static CameraConfiguration Lerp(CameraConfiguration a, CameraConfiguration b, float t)
        {
            CameraConfiguration config = new();
            config.UpdateRotation(Quaternion.Lerp(a.GetRotation(), b.GetRotation(), t));
            config.pivot = Vector3.Lerp(a.pivot, b.pivot, t);
            config.fov = Mathf.Lerp(a.fov, b.fov, t);
            config.distance = Mathf.Lerp(a.distance, b.distance, t);
            return config;
        }
    }
}