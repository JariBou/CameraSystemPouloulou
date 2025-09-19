using System.Collections.Generic;
using CameraSystem._project.Scripts.Views;
using UnityEngine;

namespace CameraSystem._project.Scripts.Volumes
{
    public class ViewVolumeBlender : MonoBehaviour
    {
        public static ViewVolumeBlender Instance { get; private set; }
        
        public List<ViewVolumeBase> _activeViewVolumes = new();
        private readonly Dictionary<ViewBase, List<ViewVolumeBase>> _volumesPerView = new();


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
            foreach (ViewBase item in CameraController.Instance.ActiveViews)
            {
                item.weight = 0;
            }
            List<ViewVolumeBase> orderedList = new List<ViewVolumeBase>(_activeViewVolumes);
            OrderListVolumes(orderedList);
            //En utilisant cette liste tri�e, pour chaque volume actif "v" :

            foreach (ViewVolumeBase v in orderedList)
            {
                // Debug.Log(v.name + " volume with " + v.Priority + " priority");
                //Calculer son poids avec � weight = v.GetSelfWeight() � et le borner entre 0 et 1,
                float weight = v.ComputeSelfWeight();
                weight = Mathf.Clamp(weight, 0.0f, 1.0f);

                //Calculer le poids restant avec: � remainingWeight = 1.0f - weight �,
                float remainingWeight = 1.0f - weight;
                //Multiplier le poids de toutes les vues actives par "remainingWeight",
                foreach (ViewBase item in CameraController.Instance.ActiveViews)
                {
                    item.weight *= remainingWeight;
                }
                //Ajouter "weight" au poids de la vue associ�e au volume.
                v.View.weight += weight;
                Debug.Log(v.View.name + " view with " + v.View.weight + " weight");
            }
        }

        private static void OrderListVolumes(List<ViewVolumeBase> outList)
        {
            outList.Sort((a, b) => a.Priority == b.Priority ? a.Uid.CompareTo(b.Uid) : a.Priority.CompareTo(b.Priority));
        }

        public void AddVolume(ViewVolumeBase volume)
        {
            _activeViewVolumes.Add(volume);

            if (_volumesPerView.TryGetValue(volume.View, out List<ViewVolumeBase> volumes))
            {
                volumes.Add(volume);
            } else
            {
                _volumesPerView.Add(volume.View, new List<ViewVolumeBase> {volume});
                volume.View.SetActive(true);
            }
        }
        
        public void RemoveVolume(ViewVolumeBase volume)
        {
            _activeViewVolumes.Remove(volume);

            if (_volumesPerView.TryGetValue(volume.View, out List<ViewVolumeBase> volumes))
            {
                volumes.Remove(volume);
                if (volumes.Count == 0)
                {
                    _volumesPerView.Remove(volume.View);
                    volume.View.SetActive(false);
                }
            }
        }
        private void OnGUI()
        {
            GUILayout.Label("Active View Volumes:");
            foreach (ViewVolumeBase volume in _activeViewVolumes)
            {
                GUILayout.Label($"{volume.GetType().Name} - Vol Prio: {volume.Priority} - View Weight:{volume.View.weight}");
            }
        }
    }
}
