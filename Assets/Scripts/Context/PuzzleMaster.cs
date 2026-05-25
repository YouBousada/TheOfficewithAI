using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    public static PuzzleMaster Instance;

    private bool TopDrawerIsOpen = false;
    private bool BottomDrawerIsOpen = false;

    public event Action OnRoomStateChanged;

    private List<ItemName> itemsInTopDrawer = new List<ItemName> {ItemName.greenPlane, ItemName.yellowPlane, ItemName.bluePlane};

    public bool hasItem(ItemName itemName)
    {
        return itemsInTopDrawer.Contains(itemName);
    }

    public bool IsTopDrawerOpen()
    {
        return TopDrawerIsOpen;
    }

    public bool IsBottomDrawerOpen()
    {
        return BottomDrawerIsOpen;
    }

    public void ToggleTopDrawer()
    {
        TopDrawerIsOpen = !TopDrawerIsOpen;
        OnRoomStateChanged?.Invoke();
    }

    public void ToggleBottomDrawer()
    {
        BottomDrawerIsOpen = !BottomDrawerIsOpen;
        OnRoomStateChanged?.Invoke();
    }

    public void removeItem(Item item)
    {
        if (itemsInTopDrawer.Contains(item.ItemName))
        {
            itemsInTopDrawer.Remove(item.ItemName);
            OnRoomStateChanged?.Invoke();
        }
    }

    void Awake()
    {
        Instance = this;
    }
}
