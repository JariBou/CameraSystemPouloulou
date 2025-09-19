using System.Collections.Generic;
using UnityEngine;

namespace CameraSystem._project.Scripts.Extensions
{
    public static class Vector3ArrayExtensions{

        public static List<Vector3> GetRange(this Vector3[] array, int start, int count)
        {
            return new List<Vector3>(array).GetRange(start, count);
        }
    }
}