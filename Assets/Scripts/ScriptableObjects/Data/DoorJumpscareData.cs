using UnityEngine;

[CreateAssetMenu(fileName = "DoorJumpscareData", menuName = "Scriptable Objects/DoorJumpscareData")]
public class DoorJumpscareData : ScriptableObject
{
    public AnimationSequence FromOtherAndDoorClosed;
    public AnimationSequence FromOtherAndDoorOpen;
    public AnimationSequence FromHere;

}
