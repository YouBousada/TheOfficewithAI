using UnityEngine;

public class CharacterIcon : MonoBehaviour
{
    [SerializeField] private RectTransform characterIcon;
    [SerializeField] private CharacterName thisCharacterName;

    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite redSprite;
    [SerializeField] private AIEvents aiMonitor;

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
            case LocName.BACK_STAGE:
                characterIcon.anchoredPosition = positions.BackStagePos;
                break;
            case LocName.SHOW_ROOM:
                characterIcon.anchoredPosition = positions.MainAreaPos;
                break;
            case LocName.PIRATES_COVE:
                characterIcon.anchoredPosition = positions.PiratesCovePos;
                break;
            case LocName.REST_ROOMS:
                characterIcon.anchoredPosition = positions.RestRoomPos;
                break;
            case LocName.CLOSET:
                characterIcon.anchoredPosition = positions.ClosetPos;
                break;
            case LocName.WEST_HALL_FAR:
                characterIcon.anchoredPosition = positions.WestHallFarPos;
                break;
            case LocName.EAST_HALL_FAR:
                characterIcon.anchoredPosition = positions.EastHallFarPos;
                break;
            case LocName.EAST_DOOR:
                characterIcon.anchoredPosition = positions.EastDoorPos;
                break;
            case LocName.WEST_DOOR:
                characterIcon.anchoredPosition = positions.WestDoorPos;
                break;
            case LocName.MANAGEMENT:
                characterIcon.anchoredPosition = positions.SecondOfficePos;
                break;
            case LocName.STAGE:
                characterIcon.anchoredPosition = positions.StagePos;
                break;
            case LocName.KITCHEN:
                characterIcon.anchoredPosition = positions.KitchenPos;
                break;
            case LocName.WEST_HALL_CORNER:
                characterIcon.anchoredPosition = positions.WestHallCornerPos;
                break;
            case LocName.EAST_HALL_CORNER:
                characterIcon.anchoredPosition = positions.EastHallCornerPos;
                break;
            case LocName.BREAKROOM:
                characterIcon.anchoredPosition = positions.BreakRoom;
                break;
            case LocName.NOWHERE:
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
            HandleRoomChange(who, location, location);
        };
    }
}
