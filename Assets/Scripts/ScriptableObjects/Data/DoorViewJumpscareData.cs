using UnityEngine;

[CreateAssetMenu(fileName = "DoorViewJumpscareData", menuName = "Scriptable Objects/DoorViewJumpscareData")]
public class DoorViewJumpscareData : ScriptableObject
{
    [Header("DoorOpen")]

        [Header("FromThisDoor")]
        public AnimationSequence ThisDoorBonnieAnimationDoorOpen;
        public AnimationSequence ThisDoorChicaAnimationDoorOpen;
        public AnimationSequence ThisDoorFoxyAnimationDoorOpen;
        public AnimationSequence ThisDoorFreddyAnimationDoorOpen;

        [Header("FromOtherDoor")]
        public AnimationSequence OtherDoorBonnieAnimationDoorOpen;
        public AnimationSequence OtherDoorChicaAnimationDoorOpen;
        public AnimationSequence OtherDoorFoxyAnimationDoorOpen;
        public AnimationSequence OtherDoorFreddyAnimationDoorOpen;

    [Header("DoorClosed")]

        [Header("FromOtherDoor")]
        public AnimationSequence OtherDoorBonnieAnimationDoorClosed;
        public AnimationSequence OtherDoorChicaAnimationDoorClosed;
        public AnimationSequence OtherDoorFoxyAnimationDoorClosed;
        public AnimationSequence OtherDoorFreddyAnimationDoorClosed;

}
