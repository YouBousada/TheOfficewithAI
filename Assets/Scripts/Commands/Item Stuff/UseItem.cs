using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Use Item")]
public class UseItem : PlayerCommand
{
    [SerializeField] private ItemVariable itemInHand;
    [SerializeField] private UseItemEvent useItemEvent;

    public override void Execute()
    {
        useItemEvent.Raise(itemInHand.item);
    }
}
