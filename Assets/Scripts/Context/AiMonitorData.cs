public struct LocationChangedData
{
    public CharacterName who;
    public RoomName from;
    public RoomName to;
    public BehaviourName behaviour;
}

public struct BehaviourChangedData
{
    public CharacterName who;
    public BehaviourName oldBehaviour;
    public BehaviourName newBehaviour;
    public RoomName location;
}

public struct JobSwitchedData
{
    public CharacterName who;
    public JobName fromJob;
    public JobName toJob;
}

public struct InitializationData
{
    public CharacterName who;
    public RoomName location;
    public BehaviourName behaviour;
    public JobName job;
}

public struct JumpscareEventData
{
    public CharacterName who;
    public RoomName from;
}
