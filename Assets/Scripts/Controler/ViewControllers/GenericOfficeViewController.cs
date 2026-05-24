using UnityEngine;
using System.Collections.Generic;

public abstract class GenericOfficeViewController : ViewController
{
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

        leftDoorJumpscares[CharacterName.Bonnie] = thisViewJumpscareData.LeftDoorBonnieAnimation;
        leftDoorJumpscares[CharacterName.Chica] = thisViewJumpscareData.LeftDoorChicaAnimation;
        leftDoorJumpscares[CharacterName.Foxy] = thisViewJumpscareData.LeftDoorFoxyAnimation;
        leftDoorJumpscares[CharacterName.Freddy] = thisViewJumpscareData.LeftDoorFreddyAnimation;

        rightDoorJumpscares[CharacterName.Bonnie] = thisViewJumpscareData.RightDoorBonnieAnimation;
        rightDoorJumpscares[CharacterName.Chica] = thisViewJumpscareData.RightDoorChicaAnimation;
        rightDoorJumpscares[CharacterName.Foxy] = thisViewJumpscareData.RightDoorFoxyAnimation;
        rightDoorJumpscares[CharacterName.Freddy] = thisViewJumpscareData.RightDoorFreddyAnimation;
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
        if (from == LocName.WestDoor) return leftDoorJumpscares;
        if (from == LocName.EastDoor) return rightDoorJumpscares;
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
