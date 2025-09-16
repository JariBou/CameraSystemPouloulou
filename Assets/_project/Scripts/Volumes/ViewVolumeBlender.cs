using CameraSystem._project.Scripts.Views;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CameraSystem
{
    public class ViewVolumeBlender : MonoBehaviour
    {
        private List<ViewVolumeBase> _activeViewVolumes;
        private Dictionary<ViewBase, List<ViewVolumeBase>> _volumesPerView;

        public static ViewVolumeBlender instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }
            instance = this;
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

    }
}
