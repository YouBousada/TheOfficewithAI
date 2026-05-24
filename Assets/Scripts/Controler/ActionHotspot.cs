using UnityEngine;

public class ActionHotspot : HotSpot
{
    [Header("ActionHotspot requirements")]
    [SerializeField] private ActionType actionType;
    [SerializeField] private HoldableItem item;

    public void InvokeAction()
    {
        if (!CanUseHotspot) return;
        if (actionType == null)
        {
            Debug.LogWarning($"{name} cannot perform an action because no PlayerAction is assigned.", this);
            return;
        }

        gameContext.Player.PerformAction(actionType);
    }
}
