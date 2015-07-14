using System;


namespace Camera_Test.Media
{
    
    public class CameraOptions : AbstractOptions {
        public CameraDevice Camera { get; set; }


        public CameraOptions() {
            this.Camera = CameraDevice.Rear;
        }
    }
}
