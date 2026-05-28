
using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Press Door Button")]
public class PressDoorButton : PlayerCommand
{   
    [SerializeField] private DoorButtonName doorButtonName;
    protected override void Execute_Concrete(ICommandContext ctx)
    {
        ctx.OfficeStates.RespondToDoorButton(doorButtonName);
    }
}