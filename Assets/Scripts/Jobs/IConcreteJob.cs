public interface IConcreteJob
{
void HandleSwitchInitialization(bool switchedFromAnotherJob);
void JobSpecificInitilizations();
}
