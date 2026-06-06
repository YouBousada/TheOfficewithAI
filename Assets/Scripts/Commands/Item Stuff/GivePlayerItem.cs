using UnityEngine;

[CreateAssetMenu(menuName = "Commands/Give Player Item")]
public class GivePlayerItem : PlayerCommand
{
    [SerializeField] private Item item;
    [SerializeField] private AttemptPickupEvent attemptPickupEvent;

    public override void Execute()
    {
        attemptPickupEvent.Raise(item);
    }
}
