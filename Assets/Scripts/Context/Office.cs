using UnityEngine;
using System;

public class Office : MonoBehaviour
{
    [SerializeField] private BooleanVariable leftDoorIsOpen;
    [SerializeField] private BooleanVariable rightDoorIsOpen;
    [SerializeField] private Boolean UsePower;

    public bool LeftDoorIsOpen => leftDoorIsOpen.boolean;
    public bool RightDoorIsOpen => rightDoorIsOpen.boolean;

    public float Power { get; private set; } = 1000f;
    private float doorDrainagePerSecond = 1f;
    private bool invokedPowerOutage = false;

    public event Action<bool, bool> OnDoorStateChanged;
    public event Action OnLeftDoorClosed;
    public event Action OnLeftDoorOpened;
    public event Action OnRightDoorClosed;
    public event Action OnRightDoorOpened;
    public event Action OnPowerOutage;

    void Awake()
    {
        leftDoorIsOpen.boolean = true;
        rightDoorIsOpen.boolean = true;
    }

    void Update()
    {
        if(!UsePower){return;}
        if (Power < 0 && !invokedPowerOutage)
        {
            OnPowerOutage?.Invoke();
            invokedPowerOutage = true;
        }

        if (!leftDoorIsOpen.boolean) Power -= Time.deltaTime * doorDrainagePerSecond;
        if (!rightDoorIsOpen.boolean) Power -= Time.deltaTime * doorDrainagePerSecond;
    }

    private void OpenLeftDoor()
    {
        if (leftDoorIsOpen.boolean) return;
        leftDoorIsOpen.boolean = true;
        OnDoorStateChanged?.Invoke(LeftDoorIsOpen, RightDoorIsOpen);
        OnLeftDoorOpened?.Invoke();
    }

    private void CloseLeftDoor()
    {
        if (!leftDoorIsOpen.boolean) return;
        leftDoorIsOpen.boolean = false;
        OnDoorStateChanged?.Invoke(LeftDoorIsOpen, RightDoorIsOpen);
        OnLeftDoorClosed?.Invoke();
    }

    private void OpenRightDoor()
    {
        if (rightDoorIsOpen.boolean) return;
        rightDoorIsOpen.boolean = true;
        OnDoorStateChanged?.Invoke(LeftDoorIsOpen, RightDoorIsOpen);
        OnRightDoorOpened?.Invoke();
    }

    private void CloseRightDoor()
    {
        if (!rightDoorIsOpen.boolean) return;
        rightDoorIsOpen.boolean = false;
        OnDoorStateChanged?.Invoke(LeftDoorIsOpen, RightDoorIsOpen);
        OnRightDoorClosed?.Invoke();
    }

    public void RespondToDoorButton(DoorButtonName doorButtonName)
    {
        switch (doorButtonName)
        {
            case DoorButtonName.LEFT_CLOSE:  CloseLeftDoor();  break;
            case DoorButtonName.LEFT_OPEN:   OpenLeftDoor();   break;
            case DoorButtonName.RIGHT_CLOSE: CloseRightDoor(); break;
            case DoorButtonName.RIGHT_OPEN:  OpenRightDoor();  break;
        }
    }


}
