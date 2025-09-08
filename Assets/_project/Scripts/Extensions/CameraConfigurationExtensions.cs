using UnityEngine;

namespace CameraSystem._project.Scripts.Extensions
{
    public static class CameraConfigurationExtensions
    {
        /// <summary>
        /// This method is <see href="https://media.tenor.com/CEO9iZ_AmygAAAAe/jeff-the-land-shark-absolute-cinema.png">this</see>
        /// </summary>
        /// <param name="self"> This </param>
        /// <param name="cameraConfiguration"> The <see cref="CameraConfiguration"/> to lerp towards </param>
        /// <param name="t"> The t of the lerp </param>
        public static void LerpTo(this CameraConfiguration self, CameraConfiguration cameraConfiguration, float t)
        {
            self.UpdateRotation(Quaternion.Lerp(self.GetRotation(), cameraConfiguration.GetRotation(), t));
            self.pivot = Vector3.Lerp(self.pivot, cameraConfiguration.pivot, t);
            self.fov =  Mathf.Lerp(self.fov, cameraConfiguration.fov, t);
        }
    }
}