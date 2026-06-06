using UnityEngine;

public class MonitorViewController : GenericOfficeViewController
{
    // [Header("MonitorViewController requirements")]
    // [SerializeField] private Sprite backgroundSprite;
    // protected override void Awake()
    // {
    //     base.Awake();
    //     if (backgroundSprite == null) Debug.LogWarning($"{name}: backgroundSprite is not assigned.", this);
    // }

    // protected override void RenderViewBasedOnWorldState_Concrete()
    // {
    //     backgroundImage.sprite = backgroundSprite;
    // }
    protected override void RenderViewBasedOnWorldState_Concrete()
    {
        throw new System.NotImplementedException();
    }
}
