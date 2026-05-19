using UnityEngine;

public class Screen : MonoBehaviour
{
    [SerializeField] private World world;
    [SerializeField] private ScreenPosition deskPosition;
    [SerializeField] private ScreenPosition closePosition;
    [SerializeField] private RectTransform rect;


    void Awake()
    {
        world.Player.OnViewChanged += WarpToView;
    }
    public void WarpToView(View newView)
    {
        if (newView == View.Desk)
        {
            ApplyDeskPosition();
        }
        else if (newView == View.Cameras)
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
}
