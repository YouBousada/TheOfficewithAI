using UnityEngine;

public abstract class PlayerCommand : ScriptableObject {
    [SerializeField] private ActionType actionType;
    public ActionType ActionType => actionType;
    protected abstract void Execute_Concrete(Player player, Office office, PuzzleMaster puzzleMaster);
    public void Execute(Player player, Office office, PuzzleMaster puzzleMaster)
    {
        player.PerformAction(actionType);
        Execute_Concrete(player, office, puzzleMaster);
    }
}