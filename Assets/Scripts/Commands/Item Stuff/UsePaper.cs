using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Use Paper")]
public class UsePaper : PlayerCommand
{
    protected override void Execute_Concrete(ICommandContext ctx)
    {
        ctx.puzzleMaster.ChangeWallWithItem(ctx.Player.itemInHand);
    }
}