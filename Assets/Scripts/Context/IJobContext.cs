public interface IJobContext
{
    AIEvents AIMonitor { get; }
    Office OfficeStates { get; }
    Player Player { get; }
    CameraSystem CameraSystem { get; }
    IAnimatronicRegistry AnimatronicRegistry { get; }
}
