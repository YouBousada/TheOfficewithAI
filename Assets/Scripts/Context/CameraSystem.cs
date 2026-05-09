using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    private LocName startingCameraRoom = LocName.Stage;
    public LocName activeCamera;
    void Start()
    {
        activeCamera = startingCameraRoom;
    }
    public void SetCameraRoom(LocName newRoom)
    {
        activeCamera = newRoom;
        OnCameraRoomChanged?.Invoke(newRoom);
    }

    public event System.Action<LocName> OnCameraRoomChanged;

}
