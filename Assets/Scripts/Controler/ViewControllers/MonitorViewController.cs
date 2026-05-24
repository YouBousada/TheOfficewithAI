using UnityEngine;

public class MonitorViewController : GenericOfficeViewController
{
    [SerializeField] private Sprite backgroundSprite;
    protected override void RenderViewBasedOnWorldState()
    {
        backgroundImage.sprite = backgroundSprite;
    }
}
