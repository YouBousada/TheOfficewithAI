using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Use Paper")]
public class UsePaper : PlayerCommand
{
    protected override void Execute_Concrete(Player player, Office office, PuzzleMaster puzzleMaster)
    {
        puzzleMaster.ChangeWallWithItem(player.itemInHand);
    }
}