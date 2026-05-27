using UnityEngine;
[RequireComponent(typeof(UnityEngine.UI.Button))]
public class MovementHotspot : HotSpot
{
    [Header("MovementHotspot requirements")]
    [SerializeField] private ViewName to;

    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(InvokeViewChange);
    }
    public void InvokeViewChange()
    {
        if (Player.Instance.IsInspectingItem)
        {
            return;
        }
        
        if (!CanUseHotspot) return;

        gameContext.Player.SetView(to);
    }
}
