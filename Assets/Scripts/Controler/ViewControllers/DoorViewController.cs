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
    protected override void ReactToJumpscare_Concrete(CharacterName who, LocName from)
    {
        if (doorViewJumpscareData == null)
        {
            return;
        }

        if (!IsDoorAttack(from))
        {
            return;
        }

        if (!isThisDoorOpen && IsAttackFromThisDoor(from))
        {
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
            return;
        }

        if (sequence == null)
        {
            return;
        }

        animPlayer.PlayAnimation(sequence, backgroundImage.gameObject);
    }

    private bool IsDoorAttack(LocName from)
    {
        return from == LocName.EAST_DOOR || from == LocName.WEST_DOOR;
    }

    private bool IsAttackFromThisDoor(LocName from)
    {
        return (from == LocName.EAST_DOOR && thisViewName == ViewName.RIGHT_DOOR) ||
               (from == LocName.WEST_DOOR && thisViewName == ViewName.LEFT_DOOR);
    }
}
