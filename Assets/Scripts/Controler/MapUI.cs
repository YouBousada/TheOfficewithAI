using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField] private Sprite bothDoorsOpen;
    [SerializeField] private Sprite bothDoorsClosed;
    [SerializeField] private Sprite rightDoorClosed;
    [SerializeField] private Sprite leftDoorClosed;

    [SerializeField] private GameContext world;

    private Image img;
    
    void Awake()
    {
        img = GetComponent<Image>();
        world.OfficeStates.OnDoorStateChanged += Refresh;
    }

    void Start() => Refresh();

    void Refresh(bool unused = false, bool unused2 = false)
    {
        if (!world.OfficeStates.LeftDoorIsOpen && !world.OfficeStates.RightDoorIsOpen) img.sprite = bothDoorsClosed;
        else if (!world.OfficeStates.LeftDoorIsOpen && world.OfficeStates.RightDoorIsOpen) img.sprite = leftDoorClosed;
        else if (world.OfficeStates.LeftDoorIsOpen && !world.OfficeStates.RightDoorIsOpen) img.sprite = rightDoorClosed;
        else img.sprite = bothDoorsOpen;
    }

    void OnDestroy()
    {
        world.OfficeStates.OnDoorStateChanged -= Refresh;
    }

}