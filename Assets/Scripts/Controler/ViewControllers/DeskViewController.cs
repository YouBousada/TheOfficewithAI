using UnityEngine;

public class DeskViewController : GenericOfficeViewController
{
    [SerializeField] private Sprite backgroundSprite;
    protected override void RenderViewBasedOnWorldState()
    {
        backgroundImage.sprite = backgroundSprite;
    }
}
