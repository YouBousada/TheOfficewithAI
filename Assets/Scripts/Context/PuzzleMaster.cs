using UnityEngine;

public class PuzzleMaster : MonoBehaviour
{
    public static PuzzleMaster Instance;
    private bool CompletedSpotDifferencePuzzle = false;
    private bool TopDrawerIsOpen = false;
    private bool BottomDrawerIsOpen = false;

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
    }

    public void ToggleBottomDrawer()
    {
        BottomDrawerIsOpen = !BottomDrawerIsOpen;
    }

    void Awake()
    {
        Instance = this;
    }
}
