using UnityEngine;

public class UnderDeskViewController : GenericOfficeViewController
{
    [Header("UnderDeskViewController requirements")]

    [Header("Renders")]
    [SerializeField] private Sprite drawerBothClosed;
    [SerializeField] private Sprite drawerBothOpen;
    [SerializeField] private Sprite drawerOnlyTopOpen;
    [SerializeField] private Sprite drawerOnlyBottomOpen;

    [Header("Shared SO Variables")]
    [SerializeField] private BooleanVariable TopDrawerIsOpen;
    [SerializeField] private BooleanVariable BottomDrawerIsOpen;
    [SerializeField] private ListOfItemsVaiable itemsInTopDrawer;

    public override void RenderViewBasedOnWorldState()
    {
        bool topOpen    = TopDrawerIsOpen.boolean;
        bool bottomOpen = BottomDrawerIsOpen.boolean;

        backgroundImage.sprite = (topOpen, bottomOpen) switch
        {
            (true,  true)  => drawerBothOpen,
            (false, false) => drawerBothClosed,
            (false, true)  => drawerOnlyBottomOpen,
            (true,  false) => drawerOnlyTopOpen,
        };
    }
}
