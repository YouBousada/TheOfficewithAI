using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Use Paper")]
public class UsePaper : PlayerCommand
{
    protected override void Execute_Concrete(GameContext ctx)
    {
        ctx.puzzleMaster.ChangeWallWithItem(ctx.Player.itemInHand);
    }
}