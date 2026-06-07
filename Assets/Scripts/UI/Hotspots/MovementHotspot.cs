using UnityEngine;
public class MovementHotspot : HotSpot
{
    [Header("MovementHotspot requirements")]
    [SerializeField] private ViewName to;
    [SerializeField] private ViewChangedEvent viewChangedEvent;

    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(InvokeViewChange);
    }
    public void InvokeViewChange()
    {
        if (!CanUseHotspot) return;
        viewChangedEvent.Raise(to);
    }
}
