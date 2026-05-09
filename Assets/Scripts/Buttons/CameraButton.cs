using UnityEngine;
using UnityEngine.UI;

public class CameraButton : MonoBehaviour
{
    public LocName thisRoom;

    public CameraSystem cameraSystem;

    public void OnButtonPressed()
    {
        Debug.Log("Camera button for " + thisRoom + " pressed.");
        cameraSystem.SetCameraRoom(thisRoom);
    }
}
