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
        [SerializeField]
        private bool _showOrbitCircles;

        internal Curve Curve => _curve;
        internal GameObject Target => _target;

        private void Update()
        {
            float axisX = 0;
            float axisY = 0;
            
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                axisX -= 1;
            } 
            if (Input.GetKey(KeyCode.RightArrow))
            {
                axisX += 1;
            } 
            if (Input.GetKey(KeyCode.DownArrow))
            {
                axisY -= 1;
            } 
            if (Input.GetKey(KeyCode.UpArrow))
            {
                axisY += 1;
            }
            
            _curvePosition = Math.Clamp(_curvePosition + axisY * Time.deltaTime, 0f, 1f);
            
            _yaw += axisX * _yawSpeed * Time.deltaTime;

            _curveToWorldMatrix = Matrix4x4.TRS(_target.transform.position, Quaternion.Euler(0, _yaw, 0), Vector3.one);
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
                 -Mathf.Asin(targetDirection.y) * Mathf.Rad2Deg,
                 0,
                 _curveToWorldMatrix.MultiplyPoint(_curve.GetPosition(_curvePosition)),
                 0,
                 50);
        }

        private void OnDrawGizmos()
        {
            if (_target != null)
            {
                _curve.DrawGizmo(transform.localToWorldMatrix, GetTargetOffset());
            }
            else
            {
                _curve.DrawGizmo(transform.localToWorldMatrix);
            }

            if (!_showOrbitCircles || _curve.points.Count < 1) return;
            
            Gizmos.color = Color.yellow;
            float tIncrement = 1f / (_curve.points.Count * 2f);
            float t = 0f;
            while (t < 1f)
            {
                Vector3 realPointCoordinates = transform.localToWorldMatrix.MultiplyPoint(_curve.GetPosition(t)) +
                                               GetTargetOffset();
                float realRadius = (realPointCoordinates - Target.transform.position.WithY(realPointCoordinates.y))
                    .magnitude;
                Vector3 realCenter = Target.transform.position.WithY(realPointCoordinates.y);

                int increment = 15;
                for (int i = 0; i < 360; i += increment)
                {
                    Gizmos.DrawLine(realCenter +
                                    new Vector3(realRadius * Mathf.Cos(i * Mathf.Deg2Rad),
                                        0,
                                        realRadius * Mathf.Sin(i * Mathf.Deg2Rad)),
                        realCenter +
                        new Vector3(realRadius * Mathf.Cos((i + increment) * Mathf.Deg2Rad),
                            0,
                            realRadius * Mathf.Sin((i + increment) * Mathf.Deg2Rad)));
                }

                t += tIncrement;
            }

            // to be sure the last one is drawn
            {
                Vector3 realPointCoordinates = transform.localToWorldMatrix.MultiplyPoint(_curve.GetPosition(1)) +
                                               GetTargetOffset();
                float realRadius = (realPointCoordinates - Target.transform.position.WithY(realPointCoordinates.y))
                    .magnitude;
                Vector3 realCenter = Target.transform.position.WithY(realPointCoordinates.y);

                int increment = 15;
                for (int i = 0; i < 360; i += increment)
                {
                    Gizmos.DrawLine(realCenter +
                                    new Vector3(realRadius * Mathf.Cos(i * Mathf.Deg2Rad),
                                        0,
                                        realRadius * Mathf.Sin(i * Mathf.Deg2Rad)),
                        realCenter +
                        new Vector3(realRadius * Mathf.Cos((i + increment) * Mathf.Deg2Rad),
                            0,
                            realRadius * Mathf.Sin((i + increment) * Mathf.Deg2Rad)));
                }
            }
        }

        public Vector3 GetTargetOffset()
        {
            return _target != null ? _target.transform.position - transform.position : Vector3.zero;
        }
    }

    public static class Vector3Extensions
    {
        public static Vector3 WithY(this Vector3 cVector3, float newY)
        {
            return new Vector3(cVector3.x, newY, cVector3.z);
        }
    }
    
}