using UnityEngine;
using UnityEngine.Serialization;

namespace CameraSystem._project.Scripts.Volumes
{
    [RequireComponent(typeof(Collider))]
    public class TriggeredViewVolume : ViewVolumeBase
    {
        [FormerlySerializedAs("targetTag")] [SerializeField]
        private string _targetTag;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(_targetTag))
            {
                SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(_targetTag))
            {
                SetActive(false);
            }
        }
    }
}