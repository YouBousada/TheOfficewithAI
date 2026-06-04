using System;
using UnityEngine;

public class CharacterIcon : MonoBehaviour
{
    [SerializeField] private RectTransform characterIcon;
    [SerializeField] private CharacterName thisCharacterName;

    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite redSprite;
    [SerializeField] private AiMonitor aiMonitor;

    [SerializeField] private CharacterUIPositions positions;

    private void Awake()
    {
        if (characterIcon == null) Debug.LogWarning($"{name}: characterIcon is not assigned.", this);
        if (aiMonitor == null)     Debug.LogWarning($"{name}: aiMonitor is not assigned.", this);
        if (positions == null)     Debug.LogWarning($"{name}: positions is not assigned.", this);
        if (greenSprite == null)   Debug.LogWarning($"{name}: greenSprite is not assigned.", this);
        if (redSprite == null)     Debug.LogWarning($"{name}: redSprite is not assigned.", this);

        if (aiMonitor == null) return;
        aiMonitor.OnLocationChanged += HandleRoomChange;
        aiMonitor.OnBehaviourChanged += HandleBehaviourChange;
        aiMonitor.OnInitialization += (who, location, behaviour, job) =>
        {
            HandleRoomChange(who, location, location, behaviour);
        };
    }

    private void HandleRoomChange(CharacterName who, RoomName from, RoomName to, BehaviourName behaviour)
    {
        if (!(who == thisCharacterName))
        {
            return;
        }

        switch (to)
        {
            case RoomName.BACK_STAGE:
                characterIcon.anchoredPosition = positions.BackStagePos;
                break;
            case RoomName.SHOW_ROOM:
                characterIcon.anchoredPosition = positions.MainAreaPos;
                break;
            case RoomName.PIRATES_COVE:
                characterIcon.anchoredPosition = positions.PiratesCovePos;
                break;
            case RoomName.REST_ROOMS:
                characterIcon.anchoredPosition = positions.RestRoomPos;
                break;
            case RoomName.CLOSET:
                characterIcon.anchoredPosition = positions.ClosetPos;
                break;
            case RoomName.WEST_HALL_FAR:
                characterIcon.anchoredPosition = positions.WestHallFarPos;
                break;
            case RoomName.EAST_HALL_FAR:
                characterIcon.anchoredPosition = positions.EastHallFarPos;
                break;
            case RoomName.EAST_DOOR:
                characterIcon.anchoredPosition = positions.EastDoorPos;
                break;
            case RoomName.WEST_DOOR:
                characterIcon.anchoredPosition = positions.WestDoorPos;
                break;
            case RoomName.MANAGEMENT:
                characterIcon.anchoredPosition = positions.SecondOfficePos;
                break;
            case RoomName.STAGE:
                characterIcon.anchoredPosition = positions.StagePos;
                break;
            case RoomName.KITCHEN:
                characterIcon.anchoredPosition = positions.KitchenPos;
                break;
            case RoomName.WEST_HALL_CORNER:
                characterIcon.anchoredPosition = positions.WestHallCornerPos;
                break;
            case RoomName.EAST_HALL_CORNER:
                characterIcon.anchoredPosition = positions.EastHallCornerPos;
                break;
            case RoomName.BREAKROOM:
                characterIcon.anchoredPosition = positions.BreakRoom;
                break;
            case RoomName.NOWHERE:
                characterIcon.anchoredPosition = positions.NoWhere;
                break;
            default:
                return;
        }
    }
        
    private void HandleBehaviourChange(CharacterName who, BehaviourName oldBehaviour, BehaviourName newBehaviour, RoomName location)
    {
        if (!(who == thisCharacterName))
        {
            return;
        }

        switch (newBehaviour)
        {
            case BehaviourName.RUNNING:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = redSprite;
                break;
            case BehaviourName.IDLING:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = greenSprite;
                break;
            case BehaviourName.ENCROACHING:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = redSprite;
                break;
            case BehaviourName.LEAVING:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = greenSprite;
                break;
            case BehaviourName.STALKING_DOOR:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = redSprite;
                break;
            case BehaviourName.STARING_THROUGH_WINDOW:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = redSprite;
                break;
            default:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = greenSprite;
                break;
        }
    }

     void OnDestroy()
    {
        aiMonitor.OnLocationChanged -= HandleRoomChange;
        aiMonitor.OnBehaviourChanged -= HandleBehaviourChange;
        aiMonitor.OnInitialization -= (who, location, behaviour, job) =>
        {
            HandleRoomChange(who, location, location, behaviour);
        };
    }


}
