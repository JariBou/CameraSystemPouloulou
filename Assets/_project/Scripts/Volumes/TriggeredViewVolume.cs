using System;
using UnityEngine;

namespace CameraSystem._project.Scripts.Volumes
{
    [RequireComponent(typeof(Collider))]
    public class TriggeredViewVolume : ViewVolumeBase
    {
        [SerializeField]
        private string targetTag;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(targetTag))
            {
                SetActive(false);
            }
        }
    }
}