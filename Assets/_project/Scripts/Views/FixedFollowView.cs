using UnityEngine;

namespace CameraSystem._project.Scripts.Views
{
    public class FixedFollowView : ViewBase
    {
        public float roll;
        public float fov;
        
        public GameObject target;

        private Vector3 GetTargetDirection()
        {
            return (target.transform.position - transform.position).normalized;
        }

        public override CameraConfiguration GetConfiguration()
        {
            Vector3 targetDirection = GetTargetDirection();
            return new CameraConfiguration(
                Mathf.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg,
                -Mathf.Asin(targetDirection.y) * Mathf.Rad2Deg,
                roll,
                transform.position,
                0,
                fov);
        }
    }
}