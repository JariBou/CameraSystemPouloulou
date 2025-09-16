using System;
using System.Collections.Generic;
using CameraSystem._project.Scripts.Views;
using UnityEngine;

namespace CameraSystem._project.Scripts.Volumes
{
    public class ViewVolumeBlender : MonoBehaviour
    {
        private List<ViewVolumeBase> _activeViewVolumes = new();
        private Dictionary<ViewBase, List<ViewVolumeBase>> _volumesPerView = new();

        public static ViewVolumeBlender Instance;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
        }

        public void AddVolume(ViewVolumeBase volume)
        {
            _activeViewVolumes.Add(volume);

            if (_volumesPerView.TryGetValue(volume.View, out List<ViewVolumeBase> volumes))
            {
                volumes.Add(volume);
            } else
            {
                _volumesPerView.Add(volume.View, new List<ViewVolumeBase>() {volume});
                volume.View.SetActive(true);
            }
        }

        public void Update()
        {
            
        }

        public void RemoveVolume(ViewVolumeBase volume)
        {
            _activeViewVolumes.Remove(volume);

            if(_volumesPerView.TryGetValue(volume.View, out List<ViewVolumeBase> volumes))
            {
                volumes.Remove(volume);
                if(volumes.Count == 0)
                {
                    volumes.Remove(volume);
                    volume.View.SetActive(false);
                }
            }
            
        }

        private void OnGUI()
        {
            GUILayout.Label("Active View Volumes:");
            foreach (ViewVolumeBase volume in _activeViewVolumes)
            {
                GUILayout.Label($"{volume.GetType().Name} - {volume.ComputeSelfWeight()}");
            }
        }
    }
}
