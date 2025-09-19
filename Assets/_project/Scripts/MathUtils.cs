using CameraSystem._project.Scripts.Extensions;
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
            t = Mathf.Clamp01(t); // Just in case, seems cleaner
            if (points == null || points.Length == 0) return Vector3.zero;
            return points.Length switch
            {
                1 => points[0],
                2 => Vector3.Lerp(points[0], points[1], t),
                _ => Vector3.Lerp(VariadicBeziers(t, points.GetRange(0, points.Length - 1).ToArray()),
                    VariadicBeziers(t, points.GetRange(1, points.Length - 1).ToArray()), t)
            };
        }

        public static Vector3 GetNearestPointOnSegment(Vector3 a, Vector3 b, Vector3 target)
        {
            Vector3 AB = b - a;
            Vector3 ABNormal = AB.normalized;
            Vector3 AC = target - a;
            // Calcul du produit scalaire entre ACible et la norme n de AB
            float scalar = (ABNormal.x * AC.x) + (ABNormal.y * AC.y) + (ABNormal.z * AC.z);
            // Borner le résultat du pro scalaire entre 0 et la distance AB
            scalar = Mathf.Clamp(scalar, 0, AB.magnitude);
            // Calculer la position la plus proche de la cible sur le segment
            // projC = A + n * produit scalaire
            Vector3 projC = a + ABNormal * scalar;
            return projC;
        }
    }
}