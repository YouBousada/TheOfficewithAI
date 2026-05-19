public class RightDoorViewController : DoorViewController
{
    protected override bool isThisDoorOpen => world.OfficeStates.RightDoorIsOpen;
    protected override Action OpenAction => Action.OpenRightDoor;
    protected override Action CloseAction => Action.CloseRightDoor;

}