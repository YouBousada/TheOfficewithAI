using UnityEngine;

[CreateAssetMenu(fileName = "WindowJumpscareData", menuName = "Scriptable Objects/WindowJumpscareData")]
public class WindowJumpscareData : ScriptableObject
{
    [Header("FromHere")]
    public AnimationSequence FromHereBonnieAnimation;
    public AnimationSequence FromHereChicaAnimation;
    public AnimationSequence FromHereFoxyAnimation;
    public AnimationSequence FromHereFreddyAnimation;

    [Header("FromOtherDoor")]
    public AnimationSequence FromOtherDoorBonnieAnimation;
    public AnimationSequence FromOtherDoorChicaAnimation;
    public AnimationSequence FromOtherDoorFoxyAnimation;
    public AnimationSequence FromOtherDoorFreddyAnimation;
}
