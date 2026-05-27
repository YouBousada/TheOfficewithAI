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

    [SerializeField] private List<Item> itemsInTopDrawer;
    public Item itemOnWall { get; private set; }

    public bool hasItem(Item item)
    {
        return itemsInTopDrawer.Contains(item);
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

    void Awake()
    {
        Instance = this;
    }
}
