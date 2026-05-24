using UnityEngine;

public class DeskCornerViewController : GenericOfficeViewController
{
    [SerializeField] private Sprite backgroundSprite;
    protected override void RenderViewBasedOnWorldState()
    {
        backgroundImage.sprite = backgroundSprite;
    }
}
