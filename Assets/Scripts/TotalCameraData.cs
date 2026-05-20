using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TotalCameraData", menuName = "Scriptable Objects/TotalCameraData")]
public class TotalCameraData : ScriptableObject
{
    public RoomCameraData[] roomCameraDatas;

    private Dictionary<LocName, RoomCameraData> roomCameraDataDictionary = new Dictionary<LocName, RoomCameraData>();

    private int FreddyStageLayer = 3;
    private int FoxyStageLayer = 1;
    private int ChicaStageLayer = 4;
    private int BonnieStageLayer = 2;

    void OnEnable()
    {
        foreach(RoomCameraData roomCamData in roomCameraDatas)
        {
            if(roomCamData == null)
            {
                Debug.LogWarning("TotalCameraData: " + roomCamData + "has a null RoomCameraData reference.");
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
        Debug.LogWarning("TotalCameraData does not contain RoomCameraData for location: " + loc);
        return null;
    }

    public int GetStageLayerForCharacter(CharacterName character)
    {
        switch(character)
        {
            case CharacterName.Freddy:
                return FreddyStageLayer;
            case CharacterName.Foxy:
                return FoxyStageLayer;
            case CharacterName.Chica:
                return ChicaStageLayer;
            case CharacterName.Bonnie:
                return BonnieStageLayer;
            default:
                Debug.LogWarning("GetStageLayerForCharacter called with invalid character: " + character);
                return -1;
        }
    }
}
