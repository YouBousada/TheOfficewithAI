using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomCameraData", menuName = "Scriptable Objects/RoomCameraData")]
public class RoomCameraData : ScriptableObject
{
    [field: SerializeField] public LocName cameraLocation { get; private set; }
    [field: SerializeField] public Sprite backgroundSprite { get; private set; }

    [Header("Each Character's Camview Data for this Camera View")]

    [SerializeField] private List<CharacterCamviewData> CharacterCamviewDataSOs;

    private Dictionary<CharacterName, CharacterCamviewData> renderDict = new Dictionary<CharacterName, CharacterCamviewData>();

    void OnEnable()
    {
        foreach(CharacterCamviewData chrDta in CharacterCamviewDataSOs)
        {
            renderDict.Add(chrDta.characterName, chrDta);
        }
    }
    public CharacterCamviewData GetCamviewDataForCharacter(CharacterName character)
    {
        if (!renderDict.TryGetValue(character, out var data) || data == null)
        {
            Debug.LogWarning($"RoomCameraData missing CamviewData for: {character}");
            return null;
        }
        return data;
    }
}
