
using UnityEngine;
[CreateAssetMenu(menuName = "Commands/Press Door Button")]
public class PressDoorButton : PlayerCommand
{   
    [SerializeField] private DoorButtonName doorButtonName;
    protected override void Execute_Concrete(Player player, Office office, PuzzleMaster puzzleMaster)
    {
        office.RespondToDoorButton(doorButtonName);
    }
}