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
        if(Player.Instance.CurrentView != thisViewName)
        {
            return;
        }
        
        switch (PuzzleMaster.Instance.itemOnWall)
        {
            case null:
                backgroundImage.sprite = nothingSprite;
                break;
            default:
                if (PuzzleMaster.Instance.itemOnWall.ItemName == ItemName.GREEN_PLANE)
                {
                    backgroundImage.sprite = greenSprite;
                }
                else if (PuzzleMaster.Instance.itemOnWall.ItemName == ItemName.YELLOW_PLANE)
                {
                    backgroundImage.sprite = yellowSprite;
                }
                else if (PuzzleMaster.Instance.itemOnWall.ItemName == ItemName.BLUE_PLANE)
                {
                    backgroundImage.sprite = blueSprite;
                }

                break;
        }
    }
}
