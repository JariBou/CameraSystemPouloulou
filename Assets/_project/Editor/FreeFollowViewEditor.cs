using CameraSystem._project.Scripts.Views;
using UnityEditor;
using UnityEngine;

namespace CameraSystem.Editor._project.Editor
{
    [CustomEditor(typeof(FreeFollowView))]
    public class FreeFollowViewEditor : UnityEditor.Editor
    {
        public FreeFollowView Target => target as FreeFollowView;
        
        
        private void OnSceneGUI()
        {
            float scale = .5f;
            Handles.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * scale);
            for (int i = 0; i < Target.Curve.points.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 point = Target.Curve.points[i];
                Vector3 positionHandle = Handles.PositionHandle(
                    (Target.GetTargetOffset() + Target.transform.localToWorldMatrix.MultiplyPoint(point)) * 1/scale, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Move Points");
                    Target.Curve.points[i] = Target.transform.worldToLocalMatrix.MultiplyPoint(
                        ((positionHandle) * scale) - Target.GetTargetOffset());
                }
            }
        }
    }
}