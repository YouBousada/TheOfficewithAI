using UnityEngine;

public class BackViewController : GenericOfficeViewController
{
    [SerializeField] private Sprite backgroundSprite;
    protected override void RenderViewBasedOnWorldState()
    {
        backgroundImage.sprite = backgroundSprite;
    }
}
