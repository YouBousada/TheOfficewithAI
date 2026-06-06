using UnityEngine;

public class DeskViewController : GenericOfficeViewController
{
    [Header("DeskViewController requirements")]
    [SerializeField] private Sprite backgroundSprite;
    protected override void Awake()
    {
        base.Awake();
        if (backgroundSprite == null) Debug.LogWarning($"{name}: backgroundSprite is not assigned.", this);
    }
    protected override void RenderViewBasedOnWorldState_Concrete()
    {
        backgroundImage.sprite = backgroundSprite;
    }
}
