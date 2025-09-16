using System;
using UnityEngine;

namespace CameraSystem._project.Scripts.Volumes
{
    public class SphereViewVolume : ViewVolumeBase
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private float _innerRadius;
        [SerializeField] private float _outerRadius;

        private float _distance;

        private void Update()
        {
            _distance = Vector3.Distance(_target.transform.position, transform.position);

            if (_distance <= _outerRadius && IsActive is false) SetActive(true);
            if (_distance > _outerRadius && IsActive) SetActive(false);
        }

        public override float ComputeSelfWeight()
        {
            // float tDistance = (_target.transform.position - transform.position).magnitude - _innerRadius;
            return Mathf.Clamp01(1 - (_distance - _innerRadius) / (_outerRadius - _innerRadius));
            // if (tDistance <= 0f) return 1f;
            return 1-Mathf.InverseLerp(_innerRadius, _outerRadius, _distance);
        }

        private void OnValidate()
        {
            if (_innerRadius > _outerRadius)
            {
                _innerRadius = _outerRadius;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _innerRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _outerRadius);
        }
    }
}