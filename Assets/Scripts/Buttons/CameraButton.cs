using UnityEngine;

public class CameraButton : MonoBehaviour
{
    [SerializeField] private LocName thisRoom;

    [SerializeField] private CameraSystem cameraSystem;

    public void OnButtonPressed()
    {
        // Debug.Log("Camera button for " + thisRoom + " pressed.");
        cameraSystem.SetCameraRoom(thisRoom);
    }
}
