using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField] private Sprite bothDoorsOpen;
    [SerializeField] private Sprite bothDoorsClosed;
    [SerializeField] private Sprite rightDoorClosed;
    [SerializeField] private Sprite leftDoorClosed;
    [SerializeField] private Office office;

    private Image img;
    
    void Awake()
    {
        img = GetComponent<Image>();
        office.OnDoorStateChanged += Refresh;
    }

    void Start() => Refresh();

    void Refresh(bool unused = false, bool unused2 = false)
    {
        if (!office.LeftDoorIsOpen && !office.RightDoorIsOpen) img.sprite = bothDoorsClosed;
        else if (!office.LeftDoorIsOpen && office.RightDoorIsOpen) img.sprite = leftDoorClosed;
        else if (office.LeftDoorIsOpen && !office.RightDoorIsOpen) img.sprite = rightDoorClosed;
        else img.sprite = bothDoorsOpen;
    }

    void OnDestroy()
    {
        office.OnDoorStateChanged -= Refresh;
    }

}
