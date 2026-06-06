using System;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    private bool TopDrawerIsOpen = false;
    private bool BottomDrawerIsOpen = false;

    public event Action OnRoomStateChanged;

    [SerializeField] private List<Item> itemsInTopDrawer;
    public Item itemOnWall { get; private set; }

    public bool hasItem(Item item)
    {
        return itemsInTopDrawer.Contains(item);
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

    public bool IsTopDrawerOpen()
    {
        return TopDrawerIsOpen;
    }

    public bool IsBottomDrawerOpen()
    {
        return BottomDrawerIsOpen;
    }

    private void ToggleTopDrawer()
    {
        TopDrawerIsOpen = !TopDrawerIsOpen;
        OnRoomStateChanged?.Invoke();
    }

    private void ToggleBottomDrawer()
    {
        BottomDrawerIsOpen = !BottomDrawerIsOpen;
        OnRoomStateChanged?.Invoke();
    }

    public void HandlePickupSuccess(Item item)
    {
        removeItem(item);
    }

    public void removeItem(Item item)
    {
        if (itemsInTopDrawer.Contains(item))
        {
            itemsInTopDrawer.Remove(item);
            OnRoomStateChanged?.Invoke();
        }
    }

    public void giveItem(Item item)
    {
        itemsInTopDrawer.Add(item);
        OnRoomStateChanged?.Invoke();
    }

    public void ChangeWallWithItem(Item item)
    {
        itemOnWall = item;
        OnRoomStateChanged?.Invoke();
    }

    void Awake() { }
}
