using System;
using System.Collections.Generic;
using UnityEngine;

namespace CameraSystem._project.Scripts
{
    [Serializable]
    public class Curve
    {
        public List<Vector3> points = new(3);
        [Range(10, 10000)]
        public float precision = 1000;

        public Color pointColor = Color.red;
        public Color curveColor = Color.white;

        
        public Vector3 GetPosition(float t)
        {
            return MathUtils.VariadicBeziers(t, points.ToArray());
        }

        public Vector3 GetPosition(float t, Matrix4x4 localToWorldMatrix)
        {
            return localToWorldMatrix.MultiplyPoint(GetPosition(t));
        }

        public void DrawGizmo(Matrix4x4 localToWorldMatrix)
        {
            Gizmos.color = pointColor;
            foreach (Vector3 point in points)
            {
                Gizmos.DrawSphere(localToWorldMatrix.MultiplyPoint(point), 0.3f);
            }

            Gizmos.color = curveColor;
            float t = 0;
            while (t < 1f)
            {
                Gizmos.DrawSphere(GetPosition(t, localToWorldMatrix), 0.1f);
                t += 1f/precision; 
            }
        }
    }
}