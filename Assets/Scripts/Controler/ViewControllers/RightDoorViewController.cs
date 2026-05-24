public class RightDoorViewController : DoorViewController
{
    protected override bool isThisDoorOpen => gameContext.OfficeStates.RightDoorIsOpen;
    protected override ActionTypeName OpenAction => ActionTypeName.OpenRightDoor;
    protected override ActionTypeName CloseAction => ActionTypeName.CloseRightDoor;

}