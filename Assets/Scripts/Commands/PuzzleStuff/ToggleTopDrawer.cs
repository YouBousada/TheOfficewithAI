using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Toggle Top Drawer")]
public class ToggleTopDrawer : PlayerCommand
{    
    protected override void Execute_Concrete(GameContext ctx)
    {
        ctx.puzzleMaster.ToggleTopDrawer();
    }
}