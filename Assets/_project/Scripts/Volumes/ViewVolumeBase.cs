using CameraSystem._project.Scripts.Views;
using UnityEngine;

namespace CameraSystem._project.Scripts.Volumes
{
    /// <summary>
    /// see: <see href="https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-classes-structs-and-interfaces">Microsoft's official doc</see> for naming choice
    /// </summary>
    public abstract class ViewVolumeBase : MonoBehaviour
    {
        public int Priority = 0;
        public ViewBase View;

        private int _uid;
        public int Uid => _uid;
        private static int NextUid;
        protected bool IsActive { get; private set; }

        [SerializeField] private bool _cutOnSwitch;

        public virtual float ComputeSelfWeight() => 1.0f;

        protected void SetActive(bool isActive)
        {
            IsActive = isActive;
            if (IsActive)
            {
                ViewVolumeBlender.Instance.AddVolume(this);
            } else
            {
                ViewVolumeBlender.Instance.RemoveVolume(this);
            }

            if (_cutOnSwitch)
            {
                ViewVolumeBlender.Instance.Update();
                CameraController.Instance.Cut();
            }

        }

        private void Awake()
        {
            _uid = NextUid++;
        }
    }
}
