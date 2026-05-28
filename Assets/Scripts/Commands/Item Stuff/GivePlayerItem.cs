using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Give Player Item")]
public class GivePlayerItem : PlayerCommand
{
    [SerializeField] Item item;

    protected override void Execute_Concrete(Player player, Office office, PuzzleMaster puzzleMaster)
    {
        if (!player.InventoryFull && !player.IsHoldingSomething)
        {
            player.AddItemToInventory(item);
            puzzleMaster.removeItem(item);
        }
    }
}