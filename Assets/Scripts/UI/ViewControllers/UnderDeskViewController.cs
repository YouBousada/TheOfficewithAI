using UnityEngine;

public class UnderDeskViewController : GenericOfficeViewController
{
    [Header("UnderDeskViewController requirements")]

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
        if (drawerBothClosed == null)    Debug.LogWarning($"{name}: drawerBothClosed is not assigned.", this);
        if (drawerBothOpen == null)      Debug.LogWarning($"{name}: drawerBothOpen is not assigned.", this);
        if (drawerOnlyTopOpen == null)   Debug.LogWarning($"{name}: drawerOnlyTopOpen is not assigned.", this);
        if (drawerOnlyBottomOpen == null) Debug.LogWarning($"{name}: drawerOnlyBottomOpen is not assigned.", this);
        if (topDrawerButton == null)     Debug.LogWarning($"{name}: topDrawerButton is not assigned.", this);
        if (bottomDrawerButton == null)  Debug.LogWarning($"{name}: bottomDrawerButton is not assigned.", this);
        if (takeBluePaper == null)       Debug.LogWarning($"{name}: takeBluePaper is not assigned.", this);
        if (takeYellowPaper == null)     Debug.LogWarning($"{name}: takeYellowPaper is not assigned.", this);
        if (takeGreenPaper == null)      Debug.LogWarning($"{name}: takeGreenPaper is not assigned.", this);
        if (bluePaper == null)           Debug.LogWarning($"{name}: bluePaper is not assigned.", this);
        if (yellowPaper == null)         Debug.LogWarning($"{name}: yellowPaper is not assigned.", this);
        if (greenPaper == null)          Debug.LogWarning($"{name}: greenPaper is not assigned.", this);
        if (bluePaperObj == null)        Debug.LogWarning($"{name}: bluePaperObj is not assigned.", this);
        if (yellowPaperObj == null)      Debug.LogWarning($"{name}: yellowPaperObj is not assigned.", this);
        if (greenPaperObj == null)       Debug.LogWarning($"{name}: greenPaperObj is not assigned.", this);
    }

    protected override void RenderViewBasedOnWorldState_Concrete()
    {
        bool topOpen    = puzzleMaster.IsTopDrawerOpen();
        bool bottomOpen = puzzleMaster.IsBottomDrawerOpen();

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
        bool itemPresent = puzzleMaster.hasItem(item);

        if (drawerOpen && itemPresent) hotspot.ActivateHotspot();
        else                           hotspot.DeactivateHotspot();

        paper.SetVisible(drawerOpen && itemPresent);
    }
}
