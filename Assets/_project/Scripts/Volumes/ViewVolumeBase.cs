using CameraSystem._project.Scripts.Views;
using UnityEngine;

namespace CameraSystem
{
    public abstract class ViewVolumeBase : MonoBehaviour
    {
        public int Priority = 0;
        public ViewBase View;

        private int Uid;
        private static int NextUid = 0;
        private static int PrevUid = 0;
        protected bool IsActive { get; private set; }

        public virtual float ComputeSelfWeight() => 1.0f;

        protected void SetActive(bool isActive)
        {
            IsActive = isActive;
            if (IsActive)
            {
                ViewVolumeBlender.instance.RemoveVolume(this);
            } else
            {
                ViewVolumeBlender.instance.AddVolume(this);
            }

        }

        private void Awake()
        {
            Uid = NextUid++;
        }
    }
}
