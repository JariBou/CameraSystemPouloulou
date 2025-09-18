using System;
using System.Collections.Generic;
using System.Linq;
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
        public void Update()
        {
            List<ViewVolumeBase> orderedList = new List<ViewVolumeBase>(_activeViewVolumes);
            OrderListVolumes(orderedList);
            //En utilisant cette liste triée, pour chaque volume actif "v" :

            foreach (ViewVolumeBase v in orderedList)
            {
                Debug.Log(v.name + " volume with " + v.Priority + " priority");
                //Calculer son poids avec « weight = v.GetSelfWeight() » et le borner entre 0 et 1,
                float weight = v.ComputeSelfWeight();
                weight = Mathf.Clamp(weight, 0.0f, 1.0f);

                //Calculer le poids restant avec: « remainingWeight = 1.0f - weight »,
                float remainingWeight = 1.0f - weight;
                //Multiplier le poids de toutes les vues actives par "remainingWeight",
                foreach (ViewVolumeBase v2 in _activeViewVolumes)
                    v2.View.weight *= remainingWeight;
                //Ajouter "weight" au poids de la vue associée au volume.
                v.View.weight += weight;
                Debug.Log(v.View.name + " view with " + v.View.weight + " weight");
            }
        }

        //_activeViewVolumes.OrderBy(o => o.Priority).ToList

        private void OrderListVolumes(List<ViewVolumeBase> outList)
        {

            outList.Sort((a, b) =>
            {
                
                if (a.Priority == b.Priority)
                {
                    return a.Uid.CompareTo(b.Uid);
                }

                return a.Priority.CompareTo(b.Priority);

            });
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
