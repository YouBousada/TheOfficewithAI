using UnityEngine;
using UnityEngine.UI;

public class Screen : OverLayObject
{
    [SerializeField] private ScreenPosition deskPosition;
    [SerializeField] private ScreenPosition closePosition;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Image image;


    void Awake()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        if (image == null)
            image = GetComponent<Image>();
    }

    protected override void Start()
    {
        base.Start();

        gameContext.Player.OnViewChanged += WarpToView;
        WarpToView(gameContext.Player.CurrentView);
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

    protected override void HideForJumpscare(CharacterName who, LocName from)
    {
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        if (image != null)
            image.enabled = visible;
    }

    protected override void OnDestroy()
    {
        if (gameContext != null)
            gameContext.Player.OnViewChanged -= WarpToView;

        base.OnDestroy();
    }
}
