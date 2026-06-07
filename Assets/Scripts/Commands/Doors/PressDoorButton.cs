
using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Press Door Button")]
public class PressDoorButton : PlayerCommand
{
    [SerializeField] private DoorButtonName doorButtonName;
    [SerializeField] private PressDoorButtonEvent pressDoorButtonEvent;
    public override void Execute()
    {
        pressDoorButtonEvent.Raise(doorButtonName);
    }
}