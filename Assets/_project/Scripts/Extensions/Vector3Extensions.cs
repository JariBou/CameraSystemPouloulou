using UnityEngine;

namespace CameraSystem._project.Scripts.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 WithY(this Vector3 cVector3, float newY)
        {
            return new Vector3(cVector3.x, newY, cVector3.z);
        }
    }
}