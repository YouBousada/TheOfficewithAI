using UnityEngine;
using UnityEngine.UI;

public class Screen : OverLayObject
{
    [Header("Screen requirements")]
    [SerializeField] private ScreenPosition deskPosition;
    [SerializeField] private ScreenPosition closePosition;
    [SerializeField] private RectTransform rect;
    private Image image;

    void Awake()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        if (image == null)
            image = GetComponent<Image>();

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

    protected override void EnterActiveView()
    {
        SetVisible(true);
    }

    protected override void ExitActiveView()
    {
        SetVisible(false);
    }

    protected override void HideForJumpscare(CharacterName who, RoomName from)
    {
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        if (image != null)
            image.enabled = visible;
    }
}
