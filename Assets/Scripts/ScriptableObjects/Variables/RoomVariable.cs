using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Room Variable")]
public class ActiveCameraVariable : ScriptableObject
{
    [SerializeField] private CameraSystemConfig cameraSystemConfig;
    public RoomName room;

    private void OnEnable()
    {
        room = cameraSystemConfig.startingRoom;
    }
}
