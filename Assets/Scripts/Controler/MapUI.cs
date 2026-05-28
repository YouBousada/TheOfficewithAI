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
        if (office == null)          Debug.LogWarning($"{name}: office is not assigned.", this);
        if (bothDoorsOpen == null)   Debug.LogWarning($"{name}: bothDoorsOpen is not assigned.", this);
        if (bothDoorsClosed == null) Debug.LogWarning($"{name}: bothDoorsClosed is not assigned.", this);
        if (rightDoorClosed == null) Debug.LogWarning($"{name}: rightDoorClosed is not assigned.", this);
        if (leftDoorClosed == null)  Debug.LogWarning($"{name}: leftDoorClosed is not assigned.", this);

        if (office == null) return;
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
