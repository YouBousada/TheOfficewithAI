using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TotalCameraData", menuName = "Scriptable Objects/TotalCameraData")]
public class TotalCameraData : ScriptableObject
{
    public RoomCameraData[] roomCameraDatas;

    private Dictionary<LocName, RoomCameraData> roomCameraDataDictionary = new Dictionary<LocName, RoomCameraData>();

    private int FreddyStageLayerIndex = 2;
    private int FoxyStageLayerIndex = 0;
    private int ChicaStageLayerIndex = 3;
    private int BonnieStageLayerIndex = 1;

    void OnEnable()
    {
        foreach(RoomCameraData roomCamData in roomCameraDatas)
        {
            if(roomCamData == null)
            {
                // Debug.LogWarning("TotalCameraData: " + roomCamData + "has a null RoomCameraData reference.");
                continue;
            }
            
            roomCameraDataDictionary[roomCamData.cameraLocation] = roomCamData;
        }
    }

    public RoomCameraData GetRoomCameraData(LocName loc)
    {
        if(roomCameraDataDictionary.TryGetValue(loc, out RoomCameraData roomCamData))
        {
            return roomCamData; 
        }
        // Debug.LogWarning("TotalCameraData does not contain RoomCameraData for location: " + loc);
        return null;
    }

    public int GetStageLayerForCharacter(CharacterName character)
    {
        switch(character)
        {
            case CharacterName.FREDDY:
                return FreddyStageLayerIndex;
            case CharacterName.FOXY:
                return FoxyStageLayerIndex;
            case CharacterName.CHICA:
                return ChicaStageLayerIndex;
            case CharacterName.BONNIE:
                return BonnieStageLayerIndex;
            default:
                // Debug.LogWarning("GetStageLayerForCharacter called with invalid character: " + character);
                return -1;
        }
    }
}
