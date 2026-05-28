using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Toggle Bottom Drawer")]
public class ToggleBottomDrawer : PlayerCommand
{    
    protected override void Execute_Concrete(Player player, Office office, PuzzleMaster puzzleMaster)
    {
        puzzleMaster.ToggleBottomDrawer();
    }
}