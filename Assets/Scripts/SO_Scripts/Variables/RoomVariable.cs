using UnityEngine;

[CreateAssetMenu(menuName = "Variables/RoomVariable")]
public class ActiveCameraVariable : ScriptableObject
{
    [SerializeField] private CameraSystemConfig cameraSystemConfig;
    public RoomName room;

    private void OnEnable()
    {
        room = cameraSystemConfig.startingRoom;
    }
}
