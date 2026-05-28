using UnityEngine;

public class LeftDoorViewController : DoorViewController
{
    [Header("LeftDoorViewController requirements")]
    [SerializeField] private Office office;

    protected override bool isThisDoorOpen => office.LeftDoorIsOpen;

    protected override void Awake()
    {
        base.Awake();
        office.OnLeftDoorOpened += PlayOpenDoorAnimation;
        office.OnLeftDoorClosed += PlayCloseDoorAnimation;
    }

    void OnDestroy()
    {
        if (office == null) return;
        office.OnLeftDoorOpened -= PlayOpenDoorAnimation;
        office.OnLeftDoorClosed -= PlayCloseDoorAnimation;
    }
}