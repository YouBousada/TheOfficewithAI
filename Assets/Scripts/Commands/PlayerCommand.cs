using UnityEngine;

public abstract class PlayerCommand : ScriptableObject {
    [SerializeField] private ActionType actionType;
    public ActionType ActionType => actionType;
    protected abstract void Execute_Concrete(ICommandContext ctx);
    public void Execute(ICommandContext ctx)
    {
        ctx.Player.PerformAction(actionType);
        Execute_Concrete(ctx);
    }
}