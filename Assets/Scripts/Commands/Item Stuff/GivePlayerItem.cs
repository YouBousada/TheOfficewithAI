using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Give Player Item")]
public class GivePlayerItem : PlayerCommand
{
    [SerializeField] Item item;

    protected override void Execute_Concrete(GameContext ctx)
    {
        if (!ctx.Player.handsFull())
        {
            ctx.Player.Give(item);
            ctx.puzzleMaster.removeItem(item);
        }
    }
}