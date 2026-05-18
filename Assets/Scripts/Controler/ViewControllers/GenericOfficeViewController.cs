using UnityEngine;
public abstract class GenericOfficeViewController : ViewController
{
    public Sprite backgroundSprite;
    public JumpscareData thsisViewJumpscareData;

    protected override void OnViewActivated_Concrete()
    {
        backgroundImage.sprite = backgroundSprite;
    }

    protected override void ReactToJumpscare_Concrete(CharacterName who, LocName from)
    {
        if (from == LocName.WestDoor)
        {
            if (who == CharacterName.Bonnie)
            {
                animPlayer.PlayAnimation(thsisViewJumpscareData.LeftDoorBonnieAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Chica)
            {
                animPlayer.PlayAnimation(thsisViewJumpscareData.LeftDoorChicaAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Foxy)
            {
                animPlayer.PlayAnimation(thsisViewJumpscareData.LeftDoorFoxyAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Freddy)
            {
                animPlayer.PlayAnimation(thsisViewJumpscareData.LeftDoorFreddyAnimation, backgroundImage.gameObject);
            }
        }
        else if (from == LocName.EastDoor)
        {
            if (who == CharacterName.Bonnie)
            {
                animPlayer.PlayAnimation(thsisViewJumpscareData.RightDoorBonnieAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Chica)
            {
                animPlayer.PlayAnimation(thsisViewJumpscareData.RightDoorChicaAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Foxy)
            {
                animPlayer.PlayAnimation(thsisViewJumpscareData.RightDoorFoxyAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Freddy)
            {
                animPlayer.PlayAnimation(thsisViewJumpscareData.RightDoorFreddyAnimation, backgroundImage.gameObject);
            }
        }
    }

    protected override void RespondToAction(Action action)
    {
        return;
    }
}