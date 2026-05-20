using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public static CameraSystem Instance { get; private set; }

    private LocName startingCameraRoom = LocName.Stage;
    public LocName ActiveCamera { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ActiveCamera = startingCameraRoom;
    }
    public void SetCameraRoom(LocName newRoom)
    {
        if(newRoom != ActiveCamera)
        {
            ActiveCamera = newRoom;
            OnCameraRoomChanged?.Invoke(newRoom);
        }
    }

    public event System.Action<LocName> OnCameraRoomChanged;

}
