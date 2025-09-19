using UnityEditor;
using UnityEngine;

namespace CameraSystem._project.Scripts
{
    /// <summary>
    /// Dummy test class for Curve, not meant to be clean (explains the #if UNITY_EDITOR)
    /// </summary>
    public class CurveTest : MonoBehaviour
    {
        public Curve curve;

        private void OnDrawGizmos()
        {
            curve.DrawGizmo(transform.localToWorldMatrix);
        }
    }
    
    #if UNITY_EDITOR


    [CustomEditor(typeof(CurveTest))]
    public class CurveTestEditor : Editor
    {
        public CurveTest Target => target as CurveTest;
        
        
        private void OnSceneGUI()
        {
            // bool isModified = false;

            float scale = .5f;
            Handles.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * scale);
            for (int i = 0; i < Target.curve.points.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                Vector3 point = Target.curve.points[i];
                Vector3 positionHandle = Handles.PositionHandle(Target.transform.localToWorldMatrix.MultiplyPoint(point) * 1/scale, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Move Points");
                    Target.curve.points[i] = Target.transform.worldToLocalMatrix.MultiplyPoint(positionHandle * scale);
                    // serializedObject.ApplyModifiedProperties();
                    // isModified = true;
                }
            }
            
            
        }
    }
    
    
    #endif
}