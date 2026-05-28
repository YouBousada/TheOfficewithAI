using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Toggle Top Drawer")]
public class ToggleTopDrawer : PlayerCommand
{    
    protected override void Execute_Concrete(ICommandContext ctx)
    {
        ctx.puzzleMaster.ToggleTopDrawer();
    }
}