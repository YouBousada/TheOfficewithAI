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
        if(newRoom != activeCamera)
        {
            activeCamera = newRoom;
            OnCameraRoomChanged?.Invoke(newRoom);
        }
    }

    public event System.Action<LocName> OnCameraRoomChanged;

}
