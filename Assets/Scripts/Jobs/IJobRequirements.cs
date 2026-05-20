public interface IJobRequirements
{
    void HandleKillPosition();
    void PerformJob();
    bool HandleNewBehaviour(Behaviour newBehaviour);
    LocName CalculateNextPos();
    void DeclareInitialization();
}
