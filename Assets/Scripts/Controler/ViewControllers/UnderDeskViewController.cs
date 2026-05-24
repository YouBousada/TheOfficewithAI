using UnityEngine;

public class UnderDeskViewController : GenericOfficeViewController
{
    [SerializeField] private Sprite DrawerBothClosed;
    [SerializeField] private Sprite DrawerBothOpen;
    [SerializeField] private Sprite DrawerOnlyTopOpen;
    [SerializeField] private Sprite DrawerOnlyBottomOpen;

    [SerializeField] private PuzzleMaster puzzleMaster;

    [SerializeField] private HotSpot OpenTopDrawerButton;
    [SerializeField] private HotSpot OpenBottomDrawerButton;

    protected override void Awake()
    {
        base.Awake();

        puzzleMaster.OnRoomStateChanged += RenderViewBasedOnWorldState;
    }

    protected override void RenderViewBasedOnWorldState()
    {

        if (puzzleMaster.IsBottomDrawerOpen() && puzzleMaster.IsTopDrawerOpen())
        {
            backgroundImage.sprite = DrawerBothOpen;
        }

        if (!puzzleMaster.IsBottomDrawerOpen() && !puzzleMaster.IsTopDrawerOpen())
        {
            backgroundImage.sprite = DrawerBothClosed;
        }

        if (!puzzleMaster.IsBottomDrawerOpen() && puzzleMaster.IsTopDrawerOpen())
        {
            backgroundImage.sprite = DrawerOnlyTopOpen;
        }

        if (puzzleMaster.IsBottomDrawerOpen() && !puzzleMaster.IsTopDrawerOpen())
        {
            backgroundImage.sprite = DrawerOnlyBottomOpen;
        }

    }



}
