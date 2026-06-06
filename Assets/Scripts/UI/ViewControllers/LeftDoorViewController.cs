public class LeftDoorViewController : DoorViewController
{

    // protected override bool isThisDoorOpen => office.LeftDoorIsOpen;

    // protected override void Awake()
    // {
    //     base.Awake();
    //     if (office == null) return;
    //     office.OnLeftDoorOpened += PlayOpenDoorAnimation;
    //     office.OnLeftDoorClosed += PlayCloseDoorAnimation;
    // }

    // void OnDestroy()
    // {
    //     if (office == null) return;
    //     office.OnLeftDoorOpened -= PlayOpenDoorAnimation;
    //     office.OnLeftDoorClosed -= PlayCloseDoorAnimation;
    // }
    protected override bool isThisDoorOpen => throw new System.NotImplementedException();
}