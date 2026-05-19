public class LeftDoorViewController : DoorViewController
{
    protected override bool isThisDoorOpen => world.OfficeStates.LeftDoorIsOpen;
    protected override Action OpenAction => Action.OpenLeftDoor;
    protected override Action CloseAction => Action.CloseLeftDoor;
}