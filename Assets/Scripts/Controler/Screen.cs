using UnityEngine;

public class Screen : MonoBehaviour
{
    public World world;
    public ScreenPosition deskPosition;
    public ScreenPosition closePosition;
    public RectTransform rect;


    void Awake()
    {
        world.playerMonitor.OnViewChanged += WarpToView;
    }
    public void WarpToView(View newView, float time)
    {
        if (newView == View.Desk)
        {
            // Invoke("ApplyDeskPosition", time / 2f);
            ApplyDeskPosition();
        }
        else if (newView == View.Cameras)
        {
            // Invoke("ApplyClosePosition", time / 2f);
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
