public interface IJobRequirements
{
    void HandleKillPosition();
    void PerformJob();
    bool HandleNewBehaviour(BehaviourName newBehaviour);
    LocName CalculateNextPos();
    void DeclareInitialization();
}
