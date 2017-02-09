using UnityEngine;
using System.Collections;
using Vuforia;

public class SwitchCamera : MonoBehaviour
{

    private CameraDevice.CameraDirection activeCameraDirection = CameraDevice.CameraDirection.CAMERA_BACK;

    public void SwitchCameraMode()
    {
        if (activeCameraDirection == CameraDevice.CameraDirection.CAMERA_BACK)
            activeCameraDirection = CameraDevice.CameraDirection.CAMERA_FRONT;

        else if (activeCameraDirection == CameraDevice.CameraDirection.CAMERA_FRONT)
            activeCameraDirection = CameraDevice.CameraDirection.CAMERA_BACK;

        CameraDevice.Instance.Stop();
        CameraDevice.Instance.Deinit();
        CameraDevice.Instance.Init(activeCameraDirection);
        CameraDevice.Instance.Start();
    }
}
