using UnityEngine;

public class LeftDoorViewController : DoorViewController
{
    [SerializeField] private BooleanVariable leftDoorIsOpen;

    protected override bool isThisDoorOpen => leftDoorIsOpen.boolean;
    protected override DoorButtonName CloseButtonName => DoorButtonName.LEFT_CLOSE;
    protected override DoorButtonName OpenButtonName => DoorButtonName.LEFT_OPEN;
}
