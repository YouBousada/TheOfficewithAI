using UnityEngine;

public class Screen : OverLayObject
{
    [Header("Screen requirements")]
    [SerializeField] private ScreenPosition deskPosition;
    [SerializeField] private ScreenPosition closePosition;
    [SerializeField] private RectTransform rect;

    protected override void Awake()
    {
        base.Awake();

        if (rect == null)
            rect = GetComponent<RectTransform>();

        if (deskPosition == null)  Debug.LogWarning($"{name}: deskPosition is not assigned.", this);
        if (closePosition == null) Debug.LogWarning($"{name}: closePosition is not assigned.", this);
    }

    protected override void Start()
    {
        base.Start();

        WarpToView(ViewName.DESK);
    }

    public void WarpToView(ViewName newView)
    {
        if (newView == ViewName.DESK)
        {
            ApplyDeskPosition();
        }
        else if (newView == ViewName.MONITOR)
        {
            ApplyClosePosition();
        }
    }

    private void ApplyDeskPosition()
    {
        Apply(deskPosition);
    }

    private void ApplyClosePosition()
    {
        Apply(closePosition);
    }

    void Apply(ScreenPosition pos)
    {
        if (pos == null) return;

        rect.anchorMin = pos.anchorMin;
        rect.anchorMax = pos.anchorMax;
        rect.pivot = pos.pivot;

        rect.anchoredPosition3D = pos.anchoredPosition;
        rect.sizeDelta = pos.sizeDelta;

        rect.localRotation = Quaternion.Euler(pos.rotation);
        rect.localScale = pos.scale;
    }

    protected override void HideForJumpscare(CharacterName who, RoomName from)
    {
        SetVisible(false);
    }
}
