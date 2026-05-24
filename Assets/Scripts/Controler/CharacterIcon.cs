using UnityEngine;

public class CharacterIcon : MonoBehaviour
{
    [SerializeField] private RectTransform characterIcon;
    [SerializeField] private CharacterName thisCharacterName;

    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite redSprite;
    [SerializeField] private GameContext gameContext;

    [SerializeField] private CharacterUIPositions positions;

    private void Awake()
    {
        gameContext.AIMonitor.OnLocationChanged += HandleRoomChange;
        gameContext.AIMonitor.OnBehaviourChanged += HandleBehaviourChange;
        gameContext.AIMonitor.OnInitialization += (who, location, behaviour, job) =>
        {
            HandleRoomChange(who, location, location);
        };
    }

    private void HandleRoomChange(CharacterName who, LocName from, LocName to)
    {
        if (!(who == thisCharacterName))
        {
            return;
        }

        switch (to)
        {
            case LocName.BackStage:
                characterIcon.anchoredPosition = positions.BackStagePos;
                break;
            case LocName.MainArea:
                characterIcon.anchoredPosition = positions.MainAreaPos;
                break;
            case LocName.PirateCove:
                characterIcon.anchoredPosition = positions.PiratesCovePos;
                break;
            case LocName.RestRooms:
                characterIcon.anchoredPosition = positions.RestRoomPos;
                break;
            case LocName.Closet:
                characterIcon.anchoredPosition = positions.ClosetPos;
                break;
            case LocName.WestHall_Far:
                characterIcon.anchoredPosition = positions.WestHallFarPos;
                break;
            case LocName.EastHall_Far:
                characterIcon.anchoredPosition = positions.EastHallFarPos;
                break;
            case LocName.EastDoor:
                characterIcon.anchoredPosition = positions.EastDoorPos;
                break;
            case LocName.WestDoor:
                characterIcon.anchoredPosition = positions.WestDoorPos;
                break;
            case LocName.SecondOffice:
                characterIcon.anchoredPosition = positions.SecondOfficePos;
                break;
            case LocName.Stage:
                characterIcon.anchoredPosition = positions.StagePos;
                break;
            case LocName.Kitchen:
                characterIcon.anchoredPosition = positions.KitchenPos;
                break;
            case LocName.WestHall_Corner:
                characterIcon.anchoredPosition = positions.WestHallCornerPos;
                break;
            case LocName.EastHall_Corner:
                characterIcon.anchoredPosition = positions.EastHallCornerPos;
                break;
            case LocName.BreakRoom:
                characterIcon.anchoredPosition = positions.BreakRoom;
                break;
            case LocName.NoWhere:
                characterIcon.anchoredPosition = positions.NoWhere;
                break;
            default:
                return;
        }
    }
        
    private void HandleBehaviourChange(CharacterName who, BehaviourName oldBehaviour, BehaviourName newBehaviour)
    {
        if (!(who == thisCharacterName))
        {
            return;
        }

        switch (newBehaviour)
        {
            case BehaviourName.Running:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = redSprite;
                break;
            case BehaviourName.Idling:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = greenSprite;
                break;
            case BehaviourName.Encroaching:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = redSprite;
                break;
            case BehaviourName.Leaving:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = greenSprite;
                break;
            case BehaviourName.StalkDoor:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = redSprite;
                break;
            case BehaviourName.StareThroughWindow:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = redSprite;
                break;
            default:
                characterIcon.GetComponent<UnityEngine.UI.Image>().sprite = greenSprite;
                break;
        }
    }

     void OnDestroy()
    {
        gameContext.AIMonitor.OnLocationChanged -= HandleRoomChange;
        gameContext.AIMonitor.OnBehaviourChanged -= HandleBehaviourChange;
        gameContext.AIMonitor.OnInitialization -= (who, location, behaviour, job) =>
        {
            HandleRoomChange(who, location, location);
        };
    }
}
