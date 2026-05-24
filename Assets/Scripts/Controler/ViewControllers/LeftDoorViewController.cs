public class LeftDoorViewController : DoorViewController
{
    protected override bool isThisDoorOpen => gameContext.OfficeStates.LeftDoorIsOpen;
    protected override ActionTypeName OpenAction => ActionTypeName.OpenLeftDoor;
    protected override ActionTypeName CloseAction => ActionTypeName.CloseLeftDoor;
}