using UnityEngine;
using System.Collections.Generic;

public abstract class GenericOfficeViewController : ViewController
{
    [SerializeField] private Sprite backgroundSprite;
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
            // Debug.LogError($"{name}: JumpscareData is not assigned.", this);
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

    protected override void OnViewActivated_Concrete()
    {
        backgroundImage.sprite = backgroundSprite;
    }

    protected override void ReactToJumpscare_Concrete(CharacterName who, LocName from)
    {
        if (thisViewJumpscareData == null)
        {
            // Debug.LogError($"{name}: Cannot play jumpscare because JumpscareData is not assigned.", this);
            return;
        }

        Dictionary<CharacterName, AnimationSequence> jumpscares = GetJumpscareLookup(from);
        if (jumpscares == null)
        {
            // Debug.LogWarning($"{name}: Generic office jumpscare called from non-door location {from}.", this);
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
            // Debug.LogWarning($"{name}: No generic office jumpscare entry for {who} from {from}.", this);
            return;
        }

        if (sequence == null)
        {
            // Debug.LogWarning($"{name}: Generic office jumpscare animation is missing for {who} from {from}.", this);
            return;
        }

        animPlayer.PlayAnimation(sequence, backgroundImage.gameObject);
    }

    protected override void RespondToAction_Concrete(PlayerAction action)
    {
        return;
    }
}
