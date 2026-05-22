using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField] private Sprite bothDoorsOpen;
    [SerializeField] private Sprite bothDoorsClosed;
    [SerializeField] private Sprite rightDoorClosed;
    [SerializeField] private Sprite leftDoorClosed;
    [SerializeField] private GameContext gameContext;

    private Image img;
    
    void Awake()
    {
        img = GetComponent<Image>();
        gameContext.OfficeStates.OnDoorStateChanged += Refresh;
    }

    void Start() => Refresh();

    void Refresh(bool unused = false, bool unused2 = false)
    {
        if (!gameContext.OfficeStates.LeftDoorIsOpen && !gameContext.OfficeStates.RightDoorIsOpen) img.sprite = bothDoorsClosed;
        else if (!gameContext.OfficeStates.LeftDoorIsOpen && gameContext.OfficeStates.RightDoorIsOpen) img.sprite = leftDoorClosed;
        else if (gameContext.OfficeStates.LeftDoorIsOpen && !gameContext.OfficeStates.RightDoorIsOpen) img.sprite = rightDoorClosed;
        else img.sprite = bothDoorsOpen;
    }

    void OnDestroy()
    {
        gameContext.OfficeStates.OnDoorStateChanged -= Refresh;
    }

}
