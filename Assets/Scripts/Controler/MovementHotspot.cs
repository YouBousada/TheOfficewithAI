using UnityEngine;

public class MovementHotspot : HotSpot
{
    [Header("MovementHotspot requirements")]
    [SerializeField] private View to;

    public void InvokeViewChange()
    {
        if (!CanUseHotspot) return;

        gameContext.Player.SetView(to);
    }
}
