public class RightDoorViewController : DoorViewController
{
    protected override bool isThisDoorOpen => gameContext.OfficeStates.RightDoorIsOpen;
    // protected override ActionTypeName OpenAction => ActionTypeName.OpenRightDoor;
    // protected override ActionTypeName CloseAction => ActionTypeName.CloseRightDoor;

    protected override void Awake()
    {
        base.Awake();
        gameContext.OfficeStates.OnRightDoorOpened += PlayOpenDoorAnimation;
        gameContext.OfficeStates.OnRightDoorClosed += PlayCloseDoorAnimation;
    }

    void OnDestroy()
    {
        if (gameContext == null || gameContext.OfficeStates == null) return;
        gameContext.OfficeStates.OnRightDoorOpened -= PlayOpenDoorAnimation;
        gameContext.OfficeStates.OnRightDoorClosed -= PlayCloseDoorAnimation;
    }
}