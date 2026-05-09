using UnityEngine;

[CreateAssetMenu(fileName = "JumpscareData", menuName = "Scriptable Objects/JumpscareData")]
public class JumpscareData : ScriptableObject
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
