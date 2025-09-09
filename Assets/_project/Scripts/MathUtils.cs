using System.Collections.Generic;
using UnityEngine;

namespace CameraSystem._project.Scripts
{
    public static class MathUtils
    {
        public static Vector3 LinearBeziers(Vector3 a, Vector3 b, float t)
        {
            return Vector3.Lerp(a, b, t);
        }
        
        public static Vector3 QuadraticBeziers(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            return Vector3.Lerp(LinearBeziers(a,b,t), LinearBeziers(b,c,t), t);
        }
        
        public static Vector3 CubicBeziers(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
        {
            return Vector3.Lerp(QuadraticBeziers(a,b,c,t), QuadraticBeziers(b,c,d,t), t);
        }

        public static Vector3 VariadicBeziers(float t, params Vector3[] points)
        {
            if (points == null || points.Length == 0) return Vector3.zero;
            if (points.Length == 1) return points[0];
            if (points.Length == 2) return Vector3.Lerp(points[0], points[1], t);
            
            return Vector3.Lerp(
                VariadicBeziers(t,
                    points.GetRange(0,
                            points.Length - 1)
                        .ToArray()),
                VariadicBeziers(t,
                    points.GetRange(1,
                            points.Length - 1)
                        .ToArray()),
                t);
        }
    }
    
    public static class Vector3ArrayExtensions{

        public static List<Vector3> GetRange(this Vector3[] array, int start, int count)
        {
            return new List<Vector3>(array).GetRange(start, count);
        }
    }
}