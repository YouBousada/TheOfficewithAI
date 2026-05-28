using UnityEngine;

public class ItemFrameViewController : GenericOfficeViewController
{
    [SerializeField] private Sprite nothingSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite yellowSprite;

    protected override void Awake()
    {
        base.Awake();
        gameContext.puzzleMaster.OnRoomStateChanged += RenderViewBasedOnWorldState;
    }
    protected override void RenderViewBasedOnWorldState()
    {
        if (gameContext.Player.CurrentView != thisViewName)
            return;

        Item wallItem = gameContext.puzzleMaster.itemOnWall;
        switch (wallItem)
        {
            case null:
                backgroundImage.sprite = nothingSprite;
                break;
            default:
                if (wallItem.ItemName == ItemName.GREEN_PLANE)
                    backgroundImage.sprite = greenSprite;
                else if (wallItem.ItemName == ItemName.YELLOW_PLANE)
                    backgroundImage.sprite = yellowSprite;
                else if (wallItem.ItemName == ItemName.BLUE_PLANE)
                    backgroundImage.sprite = blueSprite;
                break;
        }
    }
}
