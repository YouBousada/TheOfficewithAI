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
    
    protected override void OnViewActivated_Concrete()
    {
        RenderViewBasedOnWorldState();
    }

    private void RenderViewBasedOnWorldState()
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

    // protected override void RespondToAction_Concrete(ActionType action)
    // {
    //     // if(action.ActionTypeName == ActionTypeName.OpenTopDrawer)
    //     // {
    //     //     puzzleMaster.ToggleTopDrawer();

    //     //     if (puzzleMaster.IsTopDrawerOpen())
    //     //     {
    //     //         OpenBottomDrawerButton.DeactivateHotspot();
    //     //     }
    //     //     else
    //     //     {
    //     //         OpenBottomDrawerButton.ActivateHotspot();
    //     //     }

    //     //     RenderViewBasedOnWorldState();
    //     // }
        
    //     // if(action.ActionTypeName == ActionTypeName.OpenBottomDrawer)
    //     // {
    //     //     puzzleMaster.ToggleBottomDrawer();
    //     //     RenderViewBasedOnWorldState();
    //     // }
    // }
}
