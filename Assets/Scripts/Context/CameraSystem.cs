using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private LocName startingCameraRoom = LocName.STAGE;
    public static CameraSystem Instance { get; private set; }
    public LocName ActiveCamera { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ActiveCamera = startingCameraRoom;
        OnCameraRoomChanged?.Invoke(startingCameraRoom);
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
