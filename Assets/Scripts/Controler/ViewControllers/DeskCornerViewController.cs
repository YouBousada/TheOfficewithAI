using UnityEngine;

public class DeskCornerViewController : GenericOfficeViewController
{
    [Header("DeskCornerViewController requirements")]
    [SerializeField] private Sprite backgroundSprite;
    protected override void RenderViewBasedOnWorldState_Concrete()
    {
        backgroundImage.sprite = backgroundSprite;
    }
}
