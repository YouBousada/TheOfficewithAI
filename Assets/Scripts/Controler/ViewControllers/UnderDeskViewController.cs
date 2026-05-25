using Microsoft.Unity.VisualStudio.Editor;
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

    [SerializeField] private HotSpot takeBluePaper;
    [SerializeField] private HotSpot takeYellowPaper;
    [SerializeField] private HotSpot takeGreenPaper;


    [Header("Image Overlays")]
    [SerializeField] private TopPaper bluePaper;
    [SerializeField] private TopPaper YellowPaper;
    [SerializeField] private TopPaper GreenPaper;


    protected override void Awake()
    {
        base.Awake();

        gameContext.puzzleMaster.OnRoomStateChanged += RenderViewBasedOnWorldState;
    }

    protected override void RenderViewBasedOnWorldState()
    {
        if (gameContext.puzzleMaster.IsTopDrawerOpen())
        {
            if(!gameContext.puzzleMaster.hasItem(ItemName.bluePlane)){
                takeBluePaper.DeactivateHotspot();
            }
            else
            {
                takeBluePaper.ActivateHotspot();
            }
            
            if(!gameContext.puzzleMaster.hasItem(ItemName.yellowPlane)){
                takeYellowPaper.DeactivateHotspot();
            }
            else
            {
                takeYellowPaper.ActivateHotspot();
            }

            if(!gameContext.puzzleMaster.hasItem(ItemName.greenPlane)){
                takeGreenPaper.DeactivateHotspot();
            }
            else
            {
                takeGreenPaper.ActivateHotspot();
            }

            bluePaper.SetVisible(gameContext.puzzleMaster.hasItem(ItemName.bluePlane));

            YellowPaper.SetVisible(gameContext.puzzleMaster.hasItem(ItemName.yellowPlane));
            
            GreenPaper.SetVisible(gameContext.puzzleMaster.hasItem(ItemName.greenPlane));
        }

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
