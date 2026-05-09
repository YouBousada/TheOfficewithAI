using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    public Sprite bothDoorsOpen;
    public Sprite bothDoorsClosed;
    public Sprite rightDoorClosed;
    public Sprite leftDoorClosed;

    public World world;

    private Image img;
    
    void Awake()
    {
        img = GetComponent<Image>();
        world.OnDoorStateChanged += Refresh;
    }

    void Start() => Refresh();

    void Refresh(bool unused = false, bool unused2 = false)
    {
        if (!world.leftDoorIsOpen && !world.rightDoorIsOpen) img.sprite = bothDoorsClosed;
        else if (!world.leftDoorIsOpen && world.rightDoorIsOpen) img.sprite = leftDoorClosed;
        else if (world.leftDoorIsOpen && !world.rightDoorIsOpen) img.sprite = rightDoorClosed;
        else img.sprite = bothDoorsOpen;
    }

    void OnDestroy()
    {
        world.OnDoorStateChanged -= Refresh;
    }

}