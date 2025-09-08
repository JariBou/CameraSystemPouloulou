namespace CameraSystem._project.Scripts.Views
{
    public class FixedView : ViewBase
    {  
        public float Yaw;
        public float Pitch;
        public float Roll;

        public float Fov;

        public override CameraConfiguration GetConfiguration()
        {
            return new CameraConfiguration(Yaw, Pitch, Roll, transform.position, 0, Fov);
        }
    }
}