using System;
using System.Runtime.CompilerServices;
using CameraSystem._project.Scripts.Extensions;
using GraphicsLabor.Scripts.Attributes.LaborerAttributes.InspectedAttributes;
using UnityEngine;

[assembly: InternalsVisibleTo("CameraSystem.Editor")]
namespace CameraSystem._project.Scripts.Views
{
    public class FreeFollowView : ViewBase
    {
        [SerializeField]
        private Curve _curve;

        [SerializeField]
        private float _yawSpeed = 180f;

        [SerializeField] 
        private GameObject _target;
        
        private Matrix4x4 _curveToWorldMatrix;
        [SerializeField] 
        private int _fov = 50;
        [SerializeField] private float _distance;

        [SerializeField] 
        private bool _enableAdvancedOptionsEditing;
        [SerializeField, Range(0f, 1f), EnableIf(nameof(_enableAdvancedOptionsEditing))]
        private float _curvePosition = .5f;
        [SerializeField, EnableIf(nameof(_enableAdvancedOptionsEditing))]
        private float _yaw;
        [SerializeField, EnableIf(nameof(_enableAdvancedOptionsEditing))]
        private bool _showOrbitCircles;
        

        internal Curve Curve => _curve;
        internal GameObject Target => _target;

        private void Awake()
        {
            _curveToWorldMatrix = Matrix4x4.TRS(_target.transform.position, Quaternion.Euler(0, _yaw, 0), Vector3.one);
        }

        private void Update()
        {
            float axisX = 0;
            float axisY = 0;
            
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                axisX += 1;
            } 
            if (Input.GetKey(KeyCode.RightArrow))
            {
                axisX -= 1;
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
                 _distance,
                 _fov);
        }

        private void OnDrawGizmos()
        {
            Vector3 localTargetPosition = _target != null ? _target.transform.position : transform.position;
            // basically defaults to transform.localToWorldMatrix if target is null (with the added benefit to take the yaw into account)
            Matrix4x4 curveToWorldMatrix = Matrix4x4.TRS(localTargetPosition, Quaternion.Euler(0, _yaw, 0), Vector3.one);
            Vector3 targetDirection = (localTargetPosition - curveToWorldMatrix.MultiplyPoint(_curve.GetPosition(_curvePosition))).normalized;
            
            new CameraConfiguration(
                Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg,
                -Mathf.Asin(targetDirection.y) * Mathf.Rad2Deg,
                0,
                curveToWorldMatrix.MultiplyPoint(_curve.GetPosition(_curvePosition)),
                _distance,
                _fov).DrawGizmos(Color.red);

            _curve.DrawGizmo(curveToWorldMatrix);

            if (!_showOrbitCircles || _curve.points.Count < 1) return;
            
            Gizmos.color = Color.yellow;
            float tIncrement = 1f / (_curve.points.Count * 2f);
            float t = 0f;
            while (t < 1f)
            {
                Vector3 realPointCoordinates = curveToWorldMatrix.MultiplyPoint(_curve.GetPosition(t));
                float realRadius = (realPointCoordinates - localTargetPosition.WithY(realPointCoordinates.y))
                    .magnitude;
                Vector3 realCenter = localTargetPosition.WithY(realPointCoordinates.y);

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

            // to be sure the last one is drawn (quick and dirty yes ik)
            {
                Vector3 realPointCoordinates = curveToWorldMatrix.MultiplyPoint(_curve.GetPosition(1));
                
                float realRadius = (realPointCoordinates - localTargetPosition.WithY(realPointCoordinates.y))
                    .magnitude;
                Vector3 realCenter = localTargetPosition.WithY(realPointCoordinates.y);

                const int increment = 15;
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

        private void OnValidate()
        {
            switch (_yaw)
            {
                case > 180:
                    _yaw -= 360;
                    break;
                case < -180:
                    _yaw += 360;
                    break;
            }
        }
    }
}