using UnityEngine;

public class MonitorViewController : GenericOfficeViewController
{
    [Header("MonitorViewController requirements")]
    [SerializeField] private Sprite backgroundSprite;
    protected override void Awake()
    {
        base.Awake();
        if (backgroundSprite == null) Debug.LogWarning($"{name}: backgroundSprite is not assigned.", this);
    }

    public override void RenderViewBasedOnWorldState()
    {
        backgroundImage.sprite = backgroundSprite;
    }
}
