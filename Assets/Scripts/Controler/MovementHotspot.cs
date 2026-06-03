using UnityEngine;
[RequireComponent(typeof(UnityEngine.UI.Button))]
public class MovementHotspot : HotSpot
{
    [Header("MovementHotspot requirements")]
    [SerializeField] private ViewName to;

    [SerializeField] ViewChangedEvent viewChangedEvent;

    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(InvokeViewChange);
    }
    public void InvokeViewChange()
    {
        if (player.IsInspectingItem)
            return;

        if (!CanUseHotspot) return;

        player.SetView(to);
        viewChangedEvent.Raise(to);
    }
}
