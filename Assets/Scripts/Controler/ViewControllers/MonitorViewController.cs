using UnityEngine;

public class MonitorViewController : GenericOfficeViewController
{
    [Header("MonitorViewController requirements")]
    [SerializeField] private Sprite backgroundSprite;
    protected override void RenderViewBasedOnWorldState_Concrete()
    {
        backgroundImage.sprite = backgroundSprite;
    }
}
