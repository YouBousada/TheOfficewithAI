public interface IJobRequirements
{
    void HandleKillPosition();
    void PerformJob();
    bool HandleNewBehaviour(BehaviourName newBehaviour);
    RoomName CalculateNextPos();
    void DeclareInitialization();
}
