using UnityEngine;

public class BackViewController : GenericOfficeViewController
{
    [Header("BackViewController requirements")]
    [SerializeField] private Sprite nothingSprite;
    [SerializeField] private Sprite greenSprite;
    [SerializeField] private Sprite blueSprite;
    [SerializeField] private Sprite yellowSprite;

    protected override void Awake()
    {
        base.Awake();
        if (nothingSprite == null) Debug.LogWarning($"{name}: nothingSprite is not assigned.", this);
        if (greenSprite == null)   Debug.LogWarning($"{name}: greenSprite is not assigned.", this);
        if (blueSprite == null)    Debug.LogWarning($"{name}: blueSprite is not assigned.", this);
        if (yellowSprite == null)  Debug.LogWarning($"{name}: yellowSprite is not assigned.", this);
    }

    protected override void RenderViewBasedOnWorldState_Concrete()
    {
        Item wallItem = puzzleMaster.itemOnWall;
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
