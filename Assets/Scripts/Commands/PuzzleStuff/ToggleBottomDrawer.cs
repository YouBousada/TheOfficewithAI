using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Toggle Bottom Drawer")]
public class ToggleBottomDrawer : PlayerCommand
{    
    protected override void Execute_Concrete(GameContext ctx)
    {
        ctx.puzzleMaster.ToggleBottomDrawer();
    }
}