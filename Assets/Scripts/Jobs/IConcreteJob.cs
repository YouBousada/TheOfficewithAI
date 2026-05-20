public interface IConcreteJob
{
void HandleSwitchInitialization(bool switchedFromAnotherJob);
void JobSpecificInitializations();
}
