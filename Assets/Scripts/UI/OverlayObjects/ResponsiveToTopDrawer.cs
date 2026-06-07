using UnityEngine;

public class ResponsiveToTopDrawer: MonoBehaviour, IRespondsToPuzzleStatus
{
    [SerializeField] private ListOfItemsVaiable itemsintopdrawer;
    [SerializeField] private BooleanVariable topdrawerIsOpen;
    [SerializeField] private Item myItem;
    
    OverLayObject overLayObject;

    public void Awake()
    {
        overLayObject = GetComponent<OverLayObject>();
    }
    
    
    public void RenderBasedOnPuzzleStatus()
    {
        if (!topdrawerIsOpen.boolean)
        {
            overLayObject.Deactivate();
        }
        else
        {
            if (itemsintopdrawer.items.Contains(myItem))
            {
                overLayObject.Activate();
            }
            else
            {
                overLayObject.Deactivate();
            }
        }
    }
}