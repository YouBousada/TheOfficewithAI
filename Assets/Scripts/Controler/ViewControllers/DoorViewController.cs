using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class DoorViewController : ViewController
{
    public DoorViewJumpscareData doorViewJumpscareData;

    public AnimationSequence DoorOpenAnimation;
    public AnimationSequence DoorCloseAnimation;
    public Sprite DoorClosedImage;
    public Sprite DoorOpenImage;

    protected abstract bool isThisDoorOpen { get; }
    protected abstract Action OpenAction { get; }
    protected abstract Action CloseAction { get; }

    protected override void RespondToAction(Action action)
    {
        if (action == OpenAction && !isThisDoorOpen)
            animPlayer.PlayAnimation(DoorOpenAnimation, backgroundImage.gameObject, DoorOpenImage);
        else if (action == CloseAction && isThisDoorOpen)
            animPlayer.PlayAnimation(DoorCloseAnimation, backgroundImage.gameObject, DoorClosedImage);
    }

    protected override void OnViewActivated_Concrete()
    {
        if (!isThisDoorOpen)
        {
            backgroundImage.sprite = DoorClosedImage;
        }
        else
        {
            backgroundImage.sprite = DoorOpenImage;
        }
    }
    protected override void ReactToJumpscare_Concrete(CharacterName who, LocName from)
    {
        if (!isThisDoorOpen)
        {
            if (who == CharacterName.Bonnie)
            {
                animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorBonnieAnimationDoorClosed, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Chica)
            {
                animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorChicaAnimationDoorClosed, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Foxy)
            {
                animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorFoxyAnimationDoorClosed, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Freddy)
            {
                animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorFreddyAnimationDoorClosed, backgroundImage.gameObject);
            }
        }
        else
        {
            if (from == LocName.EastDoor)
            {
                if (who == CharacterName.Bonnie)
                {
                    animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorBonnieAnimationDoorOpen, backgroundImage.gameObject);
                }
                else if (who == CharacterName.Chica)
                {
                    animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorChicaAnimationDoorOpen, backgroundImage.gameObject);
                }
                else if (who == CharacterName.Foxy)
                {
                    animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorFoxyAnimationDoorOpen, backgroundImage.gameObject);
                }
                else if (who == CharacterName.Freddy)
                {
                    animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorFreddyAnimationDoorOpen, backgroundImage.gameObject);
                }
            }
            else
            {
                if (who == CharacterName.Bonnie)
                {
                    animPlayer.PlayAnimation(doorViewJumpscareData.ThisDoorBonnieAnimationDoorOpen, backgroundImage.gameObject);
                }
                else if (who == CharacterName.Chica)
                {
                    animPlayer.PlayAnimation(doorViewJumpscareData.ThisDoorChicaAnimationDoorOpen, backgroundImage.gameObject);
                }
                else if (who == CharacterName.Foxy)
                {
                    animPlayer.PlayAnimation(doorViewJumpscareData.ThisDoorFoxyAnimationDoorOpen, backgroundImage.gameObject);
                }
                else if (who == CharacterName.Freddy)
                {
                    animPlayer.PlayAnimation(doorViewJumpscareData.ThisDoorFreddyAnimationDoorOpen, backgroundImage.gameObject);
                }

            }
        }
    }
}