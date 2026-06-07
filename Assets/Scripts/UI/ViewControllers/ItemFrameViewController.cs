using UnityEngine;

public class ItemFrameViewController : GenericOfficeViewController
{
    [Header("ItemFrameViewController requirements")]
    [SerializeField] private Sprite nothingSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite yellowSprite;
    [SerializeField] ItemVariable itemOnWall;

    public override void RenderViewBasedOnWorldState()
    {
        ItemName itemOnWallName = itemOnWall.item ? itemOnWall.item.ItemName : ItemName.NONE; 

        backgroundImage.sprite = itemOnWallName switch
        {
            (ItemName.BLUE_PLANE) => blueSprite,
            (ItemName.GREEN_PLANE) => greenSprite,
            (ItemName.YELLOW_PLANE) => yellowSprite,

            _ => nothingSprite

        };
    }
}
