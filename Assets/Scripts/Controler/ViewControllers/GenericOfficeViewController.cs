using UnityEngine;
public abstract class GenericOfficeViewController : ViewController
{
    [SerializeField] private Sprite backgroundSprite;
    [SerializeField] private JumpscareData thisViewJumpscareData;

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
                animPlayer.PlayAnimation(thisViewJumpscareData.LeftDoorBonnieAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Chica)
            {
                animPlayer.PlayAnimation(thisViewJumpscareData.LeftDoorChicaAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Foxy)
            {
                animPlayer.PlayAnimation(thisViewJumpscareData.LeftDoorFoxyAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Freddy)
            {
                animPlayer.PlayAnimation(thisViewJumpscareData.LeftDoorFreddyAnimation, backgroundImage.gameObject);
            }
        }
        else if (from == LocName.EastDoor)
        {
            if (who == CharacterName.Bonnie)
            {
                animPlayer.PlayAnimation(thisViewJumpscareData.RightDoorBonnieAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Chica)
            {
                animPlayer.PlayAnimation(thisViewJumpscareData.RightDoorChicaAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Foxy)
            {
                animPlayer.PlayAnimation(thisViewJumpscareData.RightDoorFoxyAnimation, backgroundImage.gameObject);
            }
            else if (who == CharacterName.Freddy)
            {
                animPlayer.PlayAnimation(thisViewJumpscareData.RightDoorFreddyAnimation, backgroundImage.gameObject);
            }
        }
    }

    protected override void RespondToAction(Action action)
    {
        return;
    }
}