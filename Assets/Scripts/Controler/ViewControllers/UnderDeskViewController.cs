using UnityEngine;

public class UnderDeskViewController : GenericOfficeViewController
{
    [Header("Renders")]
    [SerializeField] private Sprite DrawerBothClosed;
    [SerializeField] private Sprite DrawerBothOpen;
    [SerializeField] private Sprite DrawerOnlyTopOpen;
    [SerializeField] private Sprite DrawerOnlyBottomOpen;
    

    [Header("Relavant HotSpots")]
    [SerializeField] private HotSpot TopDrawerButton;
    [SerializeField] private HotSpot BottomDrawerButton;


    protected override void Awake()
    {
        base.Awake();

        gameContext.puzzleMaster.OnRoomStateChanged += RenderViewBasedOnWorldState;
    }

    protected override void RenderViewBasedOnWorldState()
    {

        if (gameContext.puzzleMaster.IsBottomDrawerOpen() && gameContext.puzzleMaster.IsTopDrawerOpen())
        {
            backgroundImage.sprite = DrawerBothOpen;
            TopDrawerButton.ActivateHotspot();
            BottomDrawerButton.DeactivateHotspot();
        }

        if (!gameContext.puzzleMaster.IsBottomDrawerOpen() && !gameContext.puzzleMaster.IsTopDrawerOpen())
        {
            backgroundImage.sprite = DrawerBothClosed;
            TopDrawerButton.ActivateHotspot();
            BottomDrawerButton.ActivateHotspot();
        }

        if (!gameContext.puzzleMaster.IsBottomDrawerOpen() && gameContext.puzzleMaster.IsTopDrawerOpen())
        {
            backgroundImage.sprite = DrawerOnlyTopOpen;
            TopDrawerButton.ActivateHotspot();
            BottomDrawerButton.DeactivateHotspot();
        }

        if (gameContext.puzzleMaster.IsBottomDrawerOpen() && !gameContext.puzzleMaster.IsTopDrawerOpen())
        {
            backgroundImage.sprite = DrawerOnlyBottomOpen;
            TopDrawerButton.ActivateHotspot();
            BottomDrawerButton.ActivateHotspot();
        }

    }



}
