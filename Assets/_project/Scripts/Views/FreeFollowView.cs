using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[assembly: InternalsVisibleTo("CameraSystem.Editor")]
namespace CameraSystem._project.Scripts.Views
{
    public class FreeFollowView : ViewBase
    {
        [SerializeField]
        private Curve _curve;
        private float _curvePosition = .5f;

        [SerializeField]
        private float _yawSpeed = 180f;
        private float _yaw;
        [SerializeField] 
        private GameObject _target;

        private Matrix4x4 _curveToWorldMatrix;

        internal Curve Curve;
        internal GameObject Target;

        private void Update()
        {
            float axisX = Input.GetAxis("Horizontal");
            float axisY = Input.GetAxis("Vertical");
            
            _curvePosition = Math.Clamp(_curvePosition + axisY * Time.deltaTime, 0f, 1f);
            
            _yaw += axisX * _yawSpeed * Time.deltaTime;

            _curveToWorldMatrix = Matrix4x4.TRS(_target.transform.position, Quaternion.Euler(0, _yaw, 0), Vector3.one);

            // for (int i = 0; i < _curve.points.Count; i++)
            // {
            //     _curve.points[i] = curveToWorldMatrix.MultiplyPoint(_curve.points[i]);
            // }
        }
        
        private Vector3 GetTargetDirection()
        {
            return (_target.transform.position - _curveToWorldMatrix.MultiplyPoint(_curve.GetPosition(_curvePosition))).normalized;
        }
        
        public override CameraConfiguration GetConfiguration()
        {
            Vector3 targetDirection = GetTargetDirection();
            
            return new CameraConfiguration(
                 Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg,
                 // 0,
                 -Mathf.Asin(targetDirection.y) * Mathf.Rad2Deg,
                 // 0,
                 0,
                 _curveToWorldMatrix.MultiplyPoint(_curve.GetPosition(_curvePosition)),
                 0,
                 50);
        }

        private void OnDrawGizmos()
        {
            _curve.DrawGizmo(transform.localToWorldMatrix);
        }
    }
    
}