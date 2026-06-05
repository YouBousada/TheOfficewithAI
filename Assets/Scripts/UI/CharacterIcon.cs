using UnityEngine;
using UnityEngine.UI;

public class CharacterIcon : MonoBehaviour
{
    [SerializeField] private RectTransform characterIcon;
    [SerializeField] private CharacterName thisCharacterName;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite redSprite;
    [SerializeField] private CharacterUIPositions positions;

    private Image iconImage;

    private void Awake()
    {
        if (characterIcon == null) Debug.LogWarning($"{name}: characterIcon is not assigned.", this);
        if (positions == null)     Debug.LogWarning($"{name}: positions is not assigned.", this);
        if (greenSprite == null)   Debug.LogWarning($"{name}: greenSprite is not assigned.", this);
        if (redSprite == null)     Debug.LogWarning($"{name}: redSprite is not assigned.", this);

        iconImage = characterIcon.GetComponent<Image>();
    }

    public void HandleLocationChange(LocationChangedData data)
    {
        if (data.who != thisCharacterName) return;
        MoveIconTo(data.to);
    }

    public void HandleBehaviourChange(BehaviourChangedData data)
    {
        if (data.who != thisCharacterName) return;

        iconImage.sprite = data.newBehaviour switch
        {
            BehaviourName.RUNNING or
            BehaviourName.ENCROACHING or
            BehaviourName.STALKING_DOOR or
            BehaviourName.STARING_THROUGH_WINDOW => redSprite,
            _ => greenSprite
        };
    }

    public void HandleInitialization(InitializationData data)
    {
        if (data.who != thisCharacterName) return;
        MoveIconTo(data.location);
    }

    private void MoveIconTo(RoomName room)
    {
        switch (room)
        {
            case RoomName.BACK_STAGE:        characterIcon.anchoredPosition = positions.BackStagePos;      break;
            case RoomName.SHOW_ROOM:         characterIcon.anchoredPosition = positions.MainAreaPos;       break;
            case RoomName.PIRATES_COVE:      characterIcon.anchoredPosition = positions.PiratesCovePos;    break;
            case RoomName.REST_ROOMS:        characterIcon.anchoredPosition = positions.RestRoomPos;       break;
            case RoomName.CLOSET:            characterIcon.anchoredPosition = positions.ClosetPos;         break;
            case RoomName.WEST_HALL_FAR:     characterIcon.anchoredPosition = positions.WestHallFarPos;    break;
            case RoomName.EAST_HALL_FAR:     characterIcon.anchoredPosition = positions.EastHallFarPos;    break;
            case RoomName.EAST_DOOR:         characterIcon.anchoredPosition = positions.EastDoorPos;       break;
            case RoomName.WEST_DOOR:         characterIcon.anchoredPosition = positions.WestDoorPos;       break;
            case RoomName.MANAGEMENT:        characterIcon.anchoredPosition = positions.SecondOfficePos;   break;
            case RoomName.STAGE:             characterIcon.anchoredPosition = positions.StagePos;          break;
            case RoomName.KITCHEN:           characterIcon.anchoredPosition = positions.KitchenPos;        break;
            case RoomName.WEST_HALL_CORNER:  characterIcon.anchoredPosition = positions.WestHallCornerPos; break;
            case RoomName.EAST_HALL_CORNER:  characterIcon.anchoredPosition = positions.EastHallCornerPos; break;
            case RoomName.BREAKROOM:         characterIcon.anchoredPosition = positions.BreakRoom;         break;
            case RoomName.NOWHERE:           characterIcon.anchoredPosition = positions.NoWhere;           break;
        }
    }
}
