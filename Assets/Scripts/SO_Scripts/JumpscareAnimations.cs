using UnityEngine;

[CreateAssetMenu(fileName = "JumpscareAnimations", menuName = "Scriptable Objects/JumpscareAnimations")]
public class JumpscareAnimations : ScriptableObject
{
    [Header("FromLeftDoor")]
    public AnimationSequence LeftDoorBonnieAnimation;
    public AnimationSequence LeftDoorChicaAnimation;
    public AnimationSequence LeftDoorFoxyAnimation;
    public AnimationSequence LeftDoorFreddyAnimation;

    [Header("FromRightDoor")]
    public AnimationSequence RightDoorBonnieAnimation;
    public AnimationSequence RightDoorChicaAnimation;
    public AnimationSequence RightDoorFoxyAnimation;
    public AnimationSequence RightDoorFreddyAnimation;
}
