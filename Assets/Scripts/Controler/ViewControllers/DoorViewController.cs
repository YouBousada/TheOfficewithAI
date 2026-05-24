using UnityEngine;
using System.Collections.Generic;

public abstract class DoorViewController : ViewController
{
    [SerializeField] private DoorViewJumpscareData doorViewJumpscareData;
    [SerializeField] private AnimationSequence DoorOpenAnimation;
    [SerializeField] private AnimationSequence DoorCloseAnimation;
    [SerializeField] private Sprite DoorClosedImage;
    [SerializeField] private Sprite DoorOpenImage;

    [SerializeField] private HotSpot CloseButton;
    [SerializeField] private HotSpot OpenButton;

    protected abstract bool isThisDoorOpen { get; }
    protected abstract ActionTypeName OpenAction { get; }
    protected abstract ActionTypeName CloseAction { get; }

    private readonly Dictionary<CharacterName, AnimationSequence> openThisDoorJumpscares = new();
    private readonly Dictionary<CharacterName, AnimationSequence> openOtherDoorJumpscares = new();
    private readonly Dictionary<CharacterName, AnimationSequence> closedOtherDoorJumpscares = new();

    protected override void Awake()
    {
        base.Awake();
        BuildJumpscareLookups();
    }

    private void BuildJumpscareLookups()
    {
        openThisDoorJumpscares.Clear();
        openOtherDoorJumpscares.Clear();
        closedOtherDoorJumpscares.Clear();

        if (doorViewJumpscareData == null)
        {
            // Debug.LogError($"{name}: DoorViewJumpscareData is not assigned.", this);
            return;
        }

        openThisDoorJumpscares[CharacterName.Bonnie] = doorViewJumpscareData.ThisDoorBonnieAnimationDoorOpen;
        openThisDoorJumpscares[CharacterName.Chica] = doorViewJumpscareData.ThisDoorChicaAnimationDoorOpen;
        openThisDoorJumpscares[CharacterName.Foxy] = doorViewJumpscareData.ThisDoorFoxyAnimationDoorOpen;
        openThisDoorJumpscares[CharacterName.Freddy] = doorViewJumpscareData.ThisDoorFreddyAnimationDoorOpen;

        openOtherDoorJumpscares[CharacterName.Bonnie] = doorViewJumpscareData.OtherDoorBonnieAnimationDoorOpen;
        openOtherDoorJumpscares[CharacterName.Chica] = doorViewJumpscareData.OtherDoorChicaAnimationDoorOpen;
        openOtherDoorJumpscares[CharacterName.Foxy] = doorViewJumpscareData.OtherDoorFoxyAnimationDoorOpen;
        openOtherDoorJumpscares[CharacterName.Freddy] = doorViewJumpscareData.OtherDoorFreddyAnimationDoorOpen;

        closedOtherDoorJumpscares[CharacterName.Bonnie] = doorViewJumpscareData.OtherDoorBonnieAnimationDoorClosed;
        closedOtherDoorJumpscares[CharacterName.Chica] = doorViewJumpscareData.OtherDoorChicaAnimationDoorClosed;
        closedOtherDoorJumpscares[CharacterName.Foxy] = doorViewJumpscareData.OtherDoorFoxyAnimationDoorClosed;
        closedOtherDoorJumpscares[CharacterName.Freddy] = doorViewJumpscareData.OtherDoorFreddyAnimationDoorClosed;
    }

    protected override void RespondToAction_Concrete(ActionType action)
    {
        if (action.ActionTypeName == OpenAction && !isThisDoorOpen)
        {
            animPlayer.PlayAnimation(DoorOpenAnimation, backgroundImage.gameObject, DoorOpenImage);
            OpenButton.DeactivateHotspot();
            CloseButton.ActivateHotspot();
        }
        else if (action.ActionTypeName == CloseAction && isThisDoorOpen)
        {
            animPlayer.PlayAnimation(DoorCloseAnimation, backgroundImage.gameObject, DoorClosedImage);
            OpenButton.ActivateHotspot();
            CloseButton.DeactivateHotspot();
        }
            
    }

    protected override void OnViewActivated_Concrete()
    {
        if (!isThisDoorOpen)
        {
            backgroundImage.sprite = DoorClosedImage;
        }
        else
        {
            backgroundImage.sprite = DoorOpenImage;
        }
        if (isThisDoorOpen)
        {
            OpenButton.DeactivateHotspot();
            CloseButton.ActivateHotspot();
        }
        else
        {
            OpenButton.ActivateHotspot();
            CloseButton.DeactivateHotspot();
        }
    }
    protected override void ReactToJumpscare_Concrete(CharacterName who, LocName from)
    {
        if (doorViewJumpscareData == null)
        {
            // Debug.LogError($"{name}: Cannot play door jumpscare because DoorViewJumpscareData is not assigned.", this);
            return;
        }

        if (!IsDoorAttack(from))
        {
            // Debug.LogWarning($"{name}: Door jumpscare called from non-door location {from}.", this);
            return;
        }

        if (!isThisDoorOpen && IsAttackFromThisDoor(from))
        {
            // Debug.LogWarning($"{name}: Closed door view received a same-door jumpscare from {from}.", this);
            return;
        }

        Dictionary<CharacterName, AnimationSequence> jumpscares = GetJumpscareLookup(from);
        PlayJumpscareFromLookup(jumpscares, who, from);
    }

    private Dictionary<CharacterName, AnimationSequence> GetJumpscareLookup(LocName from)
    {
        if (!isThisDoorOpen)
            return closedOtherDoorJumpscares;

        return IsAttackFromThisDoor(from) ? openThisDoorJumpscares : openOtherDoorJumpscares;
    }

    private void PlayJumpscareFromLookup(Dictionary<CharacterName, AnimationSequence> jumpscares, CharacterName who, LocName from)
    {
        if (!jumpscares.TryGetValue(who, out AnimationSequence sequence))
        {
            // Debug.LogWarning($"{name}: No door jumpscare entry for {who} from {from}.", this);
            return;
        }

        if (sequence == null)
        {
            // Debug.LogWarning($"{name}: Door jumpscare animation is missing for {who} from {from}.", this);
            return;
        }

        animPlayer.PlayAnimation(sequence, backgroundImage.gameObject);
    }

    private bool IsDoorAttack(LocName from)
    {
        return from == LocName.EastDoor || from == LocName.WestDoor;
    }

    private bool IsAttackFromThisDoor(LocName from)
    {
        return (from == LocName.EastDoor && thisViewName == View.RightDoor) ||
               (from == LocName.WestDoor && thisViewName == View.LeftDoor);
    }
}
