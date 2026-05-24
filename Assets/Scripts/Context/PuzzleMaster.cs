using System;
using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    public static PuzzleMaster Instance;

    private bool TopDrawerIsOpen = false;
    private bool BottomDrawerIsOpen = false;

    public event Action OnRoomStateChanged;

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

    void Awake()
    {
        Instance = this;
    }
}
