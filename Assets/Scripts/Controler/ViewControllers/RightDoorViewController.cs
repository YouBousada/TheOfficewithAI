using UnityEngine;

public class RightDoorViewController : DoorViewController
{
    [Header("RightDoorViewController requirements")]
    [SerializeField] private Office office;

    protected override bool isThisDoorOpen => office.RightDoorIsOpen;

    protected override void Awake()
    {
        base.Awake();
        office.OnRightDoorOpened += PlayOpenDoorAnimation;
        office.OnRightDoorClosed += PlayCloseDoorAnimation;
    }

    void OnDestroy()
    {
        if (office == null) return;
        office.OnRightDoorOpened -= PlayOpenDoorAnimation;
        office.OnRightDoorClosed -= PlayCloseDoorAnimation;
    }
}