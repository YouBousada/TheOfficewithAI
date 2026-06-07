using UnityEngine;

public class RightDoorViewController : DoorViewController
{
    [SerializeField] private BooleanVariable rightDoorIsOpen;

    protected override bool isThisDoorOpen => rightDoorIsOpen.boolean;
    protected override DoorButtonName CloseButtonName => DoorButtonName.RIGHT_CLOSE;
    protected override DoorButtonName OpenButtonName => DoorButtonName.RIGHT_OPEN;
}
