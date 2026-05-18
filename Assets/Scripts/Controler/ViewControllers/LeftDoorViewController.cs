public class LeftDoorViewController : DoorViewController
{
    protected override bool isThisDoorOpen => world.leftDoorIsOpen;
    protected override Action OpenAction => Action.OpenLeftDoor;
    protected override Action CloseAction => Action.CloseLeftDoor;
}