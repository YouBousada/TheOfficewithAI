public class LeftDoorViewController : DoorViewController
{
    protected override bool isThisDoorOpen => gameContext.OfficeStates.LeftDoorIsOpen;

    protected override void Awake()
    {
        base.Awake();
        gameContext.OfficeStates.OnLeftDoorOpened += PlayOpenDoorAnimation;
        gameContext.OfficeStates.OnLeftDoorClosed += PlayCloseDoorAnimation;
    }

    void OnDestroy()
    {
        if (gameContext == null || gameContext.OfficeStates == null) return;
        gameContext.OfficeStates.OnLeftDoorOpened -= PlayOpenDoorAnimation;
        gameContext.OfficeStates.OnLeftDoorClosed -= PlayCloseDoorAnimation;
    }
}