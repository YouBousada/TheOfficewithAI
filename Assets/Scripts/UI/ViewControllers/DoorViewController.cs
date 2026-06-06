using UnityEngine;
using System.Collections.Generic;

public abstract class DoorViewController : ViewController
{
    [Header("DoorViewController requirements")]
    [SerializeField] private DoorViewJumpscareData doorViewJumpscareData;
    [SerializeField] private AnimationSequence DoorOpenAnimation;
    [SerializeField] private AnimationSequence DoorCloseAnimation;
    [SerializeField] private Sprite DoorClosedImage;
    [SerializeField] private Sprite DoorOpenImage;

    [SerializeField] private HotSpot CloseButton;
    [SerializeField] private HotSpot OpenButton;

    protected abstract bool isThisDoorOpen { get; }

    private readonly Dictionary<CharacterName, AnimationSequence> openThisDoorJumpscares = new();
    private readonly Dictionary<CharacterName, AnimationSequence> openOtherDoorJumpscares = new();
    private readonly Dictionary<CharacterName, AnimationSequence> closedOtherDoorJumpscares = new();

    protected override void Awake()
    {
        base.Awake();

        if (DoorOpenAnimation == null)  Debug.LogWarning($"{name}: DoorOpenAnimation is not assigned.", this);
        if (DoorCloseAnimation == null) Debug.LogWarning($"{name}: DoorCloseAnimation is not assigned.", this);
        if (DoorClosedImage == null)    Debug.LogWarning($"{name}: DoorClosedImage is not assigned.", this);
        if (DoorOpenImage == null)      Debug.LogWarning($"{name}: DoorOpenImage is not assigned.", this);
        if (CloseButton == null)        Debug.LogWarning($"{name}: CloseButton is not assigned.", this);
        if (OpenButton == null)         Debug.LogWarning($"{name}: OpenButton is not assigned.", this);

        BuildJumpscareLookups();
    }

    private void BuildJumpscareLookups()
    {
        openThisDoorJumpscares.Clear();
        openOtherDoorJumpscares.Clear();
        closedOtherDoorJumpscares.Clear();

        if (doorViewJumpscareData == null)
        {
            return;
        }

        openThisDoorJumpscares[CharacterName.BONNIE] = doorViewJumpscareData.ThisDoorBonnieAnimationDoorOpen;
        openThisDoorJumpscares[CharacterName.CHICA] = doorViewJumpscareData.ThisDoorChicaAnimationDoorOpen;
        openThisDoorJumpscares[CharacterName.FOXY] = doorViewJumpscareData.ThisDoorFoxyAnimationDoorOpen;
        openThisDoorJumpscares[CharacterName.FREDDY] = doorViewJumpscareData.ThisDoorFreddyAnimationDoorOpen;

        openOtherDoorJumpscares[CharacterName.BONNIE] = doorViewJumpscareData.OtherDoorBonnieAnimationDoorOpen;
        openOtherDoorJumpscares[CharacterName.CHICA] = doorViewJumpscareData.OtherDoorChicaAnimationDoorOpen;
        openOtherDoorJumpscares[CharacterName.FOXY] = doorViewJumpscareData.OtherDoorFoxyAnimationDoorOpen;
        openOtherDoorJumpscares[CharacterName.FREDDY] = doorViewJumpscareData.OtherDoorFreddyAnimationDoorOpen;

        closedOtherDoorJumpscares[CharacterName.BONNIE] = doorViewJumpscareData.OtherDoorBonnieAnimationDoorClosed;
        closedOtherDoorJumpscares[CharacterName.CHICA] = doorViewJumpscareData.OtherDoorChicaAnimationDoorClosed;
        closedOtherDoorJumpscares[CharacterName.FOXY] = doorViewJumpscareData.OtherDoorFoxyAnimationDoorClosed;
        closedOtherDoorJumpscares[CharacterName.FREDDY] = doorViewJumpscareData.OtherDoorFreddyAnimationDoorClosed;
    }

    public void PlayOpenDoorAnimation()
    {
        animPlayer.PlayAnimation(DoorOpenAnimation, backgroundImage.gameObject, DoorOpenImage);
        OpenButton.DeactivateHotspot();
        CloseButton.ActivateHotspot();
    }

    public void PlayCloseDoorAnimation()
    {
        animPlayer.PlayAnimation(DoorCloseAnimation, backgroundImage.gameObject, DoorClosedImage);
        OpenButton.ActivateHotspot();
        CloseButton.DeactivateHotspot();
    }

    protected override void RenderViewBasedOnWorldState_Concrete()
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
    public override void ReactToJumpscare(JumpscareEventData jsData)
    {
        if (doorViewJumpscareData == null)
        {
            return;
        }

        if (!IsDoorAttack(jsData.from))
        {
            return;
        }

        if (!isThisDoorOpen && IsAttackFromThisDoor(jsData.from))
        {
            return;
        }

        Dictionary<CharacterName, AnimationSequence> jumpscares = GetJumpscareLookup(jsData.from);
        PlayJumpscareFromLookup(jumpscares, jsData.who, jsData.from);
    }

    private Dictionary<CharacterName, AnimationSequence> GetJumpscareLookup(RoomName from)
    {
        if (!isThisDoorOpen)
            return closedOtherDoorJumpscares;

        return IsAttackFromThisDoor(from) ? openThisDoorJumpscares : openOtherDoorJumpscares;
    }

    private void PlayJumpscareFromLookup(Dictionary<CharacterName, AnimationSequence> jumpscares, CharacterName who, RoomName from)
    {
        if (!jumpscares.TryGetValue(who, out AnimationSequence sequence))
        {
            return;
        }

        if (sequence == null)
        {
            return;
        }

        animPlayer.PlayAnimation(sequence, backgroundImage.gameObject);
    }

    private bool IsDoorAttack(RoomName from)
    {
        return from == RoomName.EAST_DOOR || from == RoomName.WEST_DOOR;
    }

    private bool IsAttackFromThisDoor(RoomName from)
    {
        return (from == RoomName.EAST_DOOR && thisViewName == ViewName.RIGHT_DOOR) ||
               (from == RoomName.WEST_DOOR && thisViewName == ViewName.LEFT_DOOR);
    }
}
