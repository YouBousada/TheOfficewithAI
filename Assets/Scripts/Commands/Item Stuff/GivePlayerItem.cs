using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Give Player Item")]
public class GivePlayerItem : PlayerCommand
{
    [SerializeField] Item item;

    public override void Execute()
    {
        // if (!player.InventoryFull && !player.IsHoldingSomething)
        // {
        //     player.AddItemToInventory(item);
        //     puzzleMaster.removeItem(item);
        // }
    }
}