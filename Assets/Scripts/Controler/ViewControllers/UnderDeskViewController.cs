using UnityEngine;

public class UnderDeskViewController : GenericOfficeViewController
{
    [Header("Renders")]
    [SerializeField] private Sprite drawerBothClosed;
    [SerializeField] private Sprite drawerBothOpen;
    [SerializeField] private Sprite drawerOnlyTopOpen;
    [SerializeField] private Sprite drawerOnlyBottomOpen;

    [Header("Relevant HotSpots")]
    [SerializeField] private HotSpot topDrawerButton;
    [SerializeField] private HotSpot bottomDrawerButton;
    [SerializeField] private HotSpot takeBluePaper;
    [SerializeField] private HotSpot takeYellowPaper;
    [SerializeField] private HotSpot takeGreenPaper;

    [Header("Image Overlays")]
    [SerializeField] private TopPaper bluePaper;
    [SerializeField] private TopPaper yellowPaper;
    [SerializeField] private TopPaper greenPaper;

    [Header("Aquireable Items In this view")]
    [SerializeField] private Item bluePaperObj;
    [SerializeField] private Item yellowPaperObj;
    [SerializeField] private Item greenPaperObj;

    protected override void Awake()
    {
        base.Awake();
        gameContext.puzzleMaster.OnRoomStateChanged += RenderViewBasedOnWorldState;
    }

    protected override void RenderViewBasedOnWorldState()
    {
        if (gameContext.Player.CurrentView != thisViewName)
            return;
        
        bool topOpen    = gameContext.puzzleMaster.IsTopDrawerOpen();
        bool bottomOpen = gameContext.puzzleMaster.IsBottomDrawerOpen();

        UpdateDrawers(topOpen, bottomOpen);
        UpdateItems(topOpen);
    }

    private void UpdateDrawers(bool topOpen, bool bottomOpen)
    {
        backgroundImage.sprite = (topOpen, bottomOpen) switch
        {
            (true,  true)  => drawerBothOpen,
            (true,  false) => drawerOnlyTopOpen,
            (false, true)  => drawerOnlyBottomOpen,
            (false, false) => drawerBothClosed,
        };

        topDrawerButton.ActivateHotspot();

        if (topOpen) bottomDrawerButton.DeactivateHotspot();
        else         bottomDrawerButton.ActivateHotspot();
    }

    private void UpdateItems(bool topOpen)
    {
        UpdateItem(takeBluePaper,   bluePaper,   bluePaperObj,   topOpen);
        UpdateItem(takeYellowPaper, yellowPaper, yellowPaperObj, topOpen);
        UpdateItem(takeGreenPaper,  greenPaper,  greenPaperObj,  topOpen);
    }

    private void UpdateItem(HotSpot hotspot, TopPaper paper, Item item, bool drawerOpen)
    {
        bool itemPresent = gameContext.puzzleMaster.hasItem(item);

        if (drawerOpen && itemPresent) hotspot.ActivateHotspot();
        else                           hotspot.DeactivateHotspot();

        paper.SetVisible(drawerOpen && itemPresent);
    }
}
