using UnityEngine;

namespace CameraSystem._project.Scripts.Views
{
    /// <summary>
    /// see: <see href="https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/names-of-classes-structs-and-interfaces">Microsoft's official doc</see> for naming choice
    /// </summary>
    public abstract class ViewBase : MonoBehaviour
    {
        public float weight;


        public virtual CameraConfiguration GetConfiguration()
        {
            return new CameraConfiguration();
        }

        public virtual void SetActive(bool isActive)
        {
            if (isActive)
            {
                CameraController.Instance.AddView(this);
            }
            else
            {
                CameraController.Instance.RemoveView(this);
            }
        }

        private void OnDrawGizmos()
        {
            GetConfiguration().DrawGizmos(Color.red);
        }
    }
}