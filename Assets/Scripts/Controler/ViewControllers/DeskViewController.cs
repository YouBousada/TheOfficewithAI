using UnityEngine;

public class DeskViewController : GenericOfficeViewController
{
    [Header("DeskViewController requirements")]
    [SerializeField] private Sprite backgroundSprite;
    protected override void RenderViewBasedOnWorldState_Concrete()
    {
        backgroundImage.sprite = backgroundSprite;
    }
}
