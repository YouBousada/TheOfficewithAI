using UnityEngine;

[CreateAssetMenu(fileName = "RoomCameraData", menuName = "Scriptable Objects/RoomCameraData")]
public class RoomCameraData : ScriptableObject
{
    [SerializeField] private LocName cameraLocation;
    [SerializeField] private Sprite backgroundSprite;

    [Header("Each Character's Camview Data for this Camera View")]
    [SerializeField] private CharacterCamviewData BonnieCamviewData;
    [SerializeField] private CharacterCamviewData ChicaCamviewData;
    [SerializeField] private CharacterCamviewData FoxyCamviewData;
    [SerializeField] private CharacterCamviewData FreddyCamviewData;

    public CharacterCamviewData GetCamviewDataForCharacter(CharacterName character)
    {
        switch(character)
        {
            case CharacterName.Bonnie:
                if(BonnieCamviewData == null)
                {
                    Debug.LogWarning("BonnieCamviewData is null for RoomCameraData at location: " + cameraLocation);
                    return null;
                }   

                return BonnieCamviewData;

            case CharacterName.Chica:
                if(ChicaCamviewData == null)
                {
                    Debug.LogWarning("ChicaCamviewData is null for RoomCameraData at location: " + cameraLocation);
                    return null;
                }

                return ChicaCamviewData;
                
            case CharacterName.Foxy:

                if(FoxyCamviewData == null)
                {
                    Debug.LogWarning("FoxyCamviewData is null for RoomCameraData at location: " + cameraLocation);
                    return null;
                }

                return FoxyCamviewData;

            case CharacterName.Freddy:
            
                if(FreddyCamviewData == null)
                {
                    Debug.LogWarning("FreddyCamviewData is null for RoomCameraData at location: " + cameraLocation);
                    return null;
                }

                return FreddyCamviewData;

            default:
                Debug.LogWarning("RoomCameraData does not have CamviewData for character: " + character);
                return null;
        }
    }


    public Sprite GetBackgroundSprite()
    {
        return backgroundSprite;
    }

    public LocName GetCameraLocation()
    {
        return cameraLocation;
    }
}
