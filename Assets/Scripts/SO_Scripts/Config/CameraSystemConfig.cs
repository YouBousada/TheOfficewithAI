using UnityEngine;

[CreateAssetMenu(fileName = "CameraSystemConfig", menuName = "Config/CameraSystemConfig")]
public class CameraSystemConfig : ScriptableObject
{
    [SerializeField] private RoomName StartingRoom = RoomName.SHOW_ROOM;
    public RoomName startingRoom => StartingRoom;
}
