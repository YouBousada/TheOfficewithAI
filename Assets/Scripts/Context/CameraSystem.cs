using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private RoomName startingCameraRoom = RoomName.STAGE;
    public static CameraSystem Instance { get; private set; }
    public RoomName ActiveCamera { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ActiveCamera = startingCameraRoom;
        OnCameraRoomChanged?.Invoke(startingCameraRoom);
    }
    public void SetCameraRoom(RoomName newRoom)
    {
        if(newRoom != ActiveCamera)
        {
            ActiveCamera = newRoom;
            OnCameraRoomChanged?.Invoke(newRoom);
        }
    }
    public event System.Action<RoomName> OnCameraRoomChanged;

}
