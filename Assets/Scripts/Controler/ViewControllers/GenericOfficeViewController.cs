using UnityEngine;
using System.Collections.Generic;

public abstract class GenericOfficeViewController : ViewController
{
    [Header("GenericOfficeViewController requirements")]
    [SerializeField] private JumpscareData thisViewJumpscareData;

    private readonly Dictionary<CharacterName, AnimationSequence> leftDoorJumpscares = new();
    private readonly Dictionary<CharacterName, AnimationSequence> rightDoorJumpscares = new();

    protected override void Awake()
    {
        base.Awake();
        BuildJumpscareLookups();
    }

    private void BuildJumpscareLookups()
    {
        leftDoorJumpscares.Clear();
        rightDoorJumpscares.Clear();

        if (thisViewJumpscareData == null)
        {
            return;
        }

        leftDoorJumpscares[CharacterName.BONNIE] = thisViewJumpscareData.LeftDoorBonnieAnimation;
        leftDoorJumpscares[CharacterName.CHICA] = thisViewJumpscareData.LeftDoorChicaAnimation;
        leftDoorJumpscares[CharacterName.FOXY] = thisViewJumpscareData.LeftDoorFoxyAnimation;
        leftDoorJumpscares[CharacterName.FREDDY] = thisViewJumpscareData.LeftDoorFreddyAnimation;

        rightDoorJumpscares[CharacterName.BONNIE] = thisViewJumpscareData.RightDoorBonnieAnimation;
        rightDoorJumpscares[CharacterName.CHICA] = thisViewJumpscareData.RightDoorChicaAnimation;
        rightDoorJumpscares[CharacterName.FOXY] = thisViewJumpscareData.RightDoorFoxyAnimation;
        rightDoorJumpscares[CharacterName.FREDDY] = thisViewJumpscareData.RightDoorFreddyAnimation;
    }

    protected override void ReactToJumpscare_Concrete(CharacterName who, LocName from)
    {
        if (thisViewJumpscareData == null)
        {
            return;
        }

        Dictionary<CharacterName, AnimationSequence> jumpscares = GetJumpscareLookup(from);
        if (jumpscares == null)
        {
            return;
        }

        PlayJumpscareFromLookup(jumpscares, who, from);
    }

    private Dictionary<CharacterName, AnimationSequence> GetJumpscareLookup(LocName from)
    {
        if (from == LocName.WEST_DOOR) return leftDoorJumpscares;
        if (from == LocName.EAST_DOOR) return rightDoorJumpscares;
        return null;
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
}
