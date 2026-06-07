using System.Collections.Generic;
using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    [Header("Shared SO Variables")]
    [SerializeField] private BooleanVariable TopDrawerIsOpen;
    [SerializeField] private BooleanVariable BottomDrawerIsOpen;
    [SerializeField] private ListOfItemsVaiable itemsInTopDrawer;
    [SerializeField] private ItemVariable itemOnWall;

    [Header("Starting State")]
    [SerializeField] private List<Item> startingItemsInTopDrawer;

    [Header("SO Events")]

    [SerializeField] private EmptyGameEvent OnRoomStatusChanged;
    public void Awake()
    {
        TopDrawerIsOpen.boolean = false;
        BottomDrawerIsOpen.boolean = false;

        itemsInTopDrawer.items.Clear();
        itemsInTopDrawer.items.AddRange(startingItemsInTopDrawer);
        itemOnWall.item = null;
    }


    public bool hasItem(Item item)
    {
        return itemsInTopDrawer.items.Contains(item);
    }

    public void HandleEnvrionementInteraction(EvironmentInteractionName i)
    {
        switch (i)
        {
            case EvironmentInteractionName.TOGGLE_TOP_DRAWER:
            ToggleTopDrawer();
            break;
            case EvironmentInteractionName.TOGGLE_BOTTOM_DRAWER:
            ToggleBottomDrawer();
            break;
        }
    }

    public bool IsTopDrawerOpen() => TopDrawerIsOpen.boolean;
    public bool IsBottomDrawerOpen() => BottomDrawerIsOpen.boolean;

    private void ToggleTopDrawer()
    {
        TopDrawerIsOpen.boolean = !TopDrawerIsOpen.boolean;
        OnRoomStatusChanged.Raise();
    }

    private void ToggleBottomDrawer()
    {
        BottomDrawerIsOpen.boolean = !BottomDrawerIsOpen.boolean;
        OnRoomStatusChanged.Raise();
    }

    public void HandlePickupSuccess(Item item)
    {
        removeItem(item);
    }

    public void removeItem(Item item)
    {
        if (itemsInTopDrawer.items.Contains(item))
        {
            itemsInTopDrawer.items.Remove(item);
            OnRoomStatusChanged.Raise();
        }
    }

    public void giveItem(Item item)
    {
        itemsInTopDrawer.items.Add(item);
        OnRoomStatusChanged.Raise();
    }

    public void ChangeWallWithItem(Item item)
    {
        itemOnWall.item = item;
        OnRoomStatusChanged.Raise();
    }
}
