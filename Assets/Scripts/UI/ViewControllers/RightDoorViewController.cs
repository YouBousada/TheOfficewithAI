public class RightDoorViewController : DoorViewController
{

    // protected override bool isThisDoorOpen => office.RightDoorIsOpen;

    // protected override void Awake()
    // {
    //     base.Awake();
    //     if (office == null) return;
    //     office.OnRightDoorOpened += PlayOpenDoorAnimation;
    //     office.OnRightDoorClosed += PlayCloseDoorAnimation;
    // }

    // void OnDestroy()
    // {
    //     if (office == null) return;
    //     office.OnRightDoorOpened -= PlayOpenDoorAnimation;
    //     office.OnRightDoorClosed -= PlayCloseDoorAnimation;
    // }
    protected override bool isThisDoorOpen => throw new System.NotImplementedException();
}