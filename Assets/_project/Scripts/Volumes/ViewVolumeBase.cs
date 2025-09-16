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

        public virtual float ComputeSelfWeight() => 1.0f;
        private void Awake()
        {
            Uid = NextUid++;
        }
    }
}
