using UnityEngine;

public abstract class PlayerCommand : ScriptableObject {
    [SerializeField] private ActionType actionType;
    protected abstract void Execute_Concrete(GameContext ctx);
    public void Execute(GameContext ctx)
    {
        ctx.Player.PerformAction(actionType);
        Execute_Concrete(ctx);
    }
    
}