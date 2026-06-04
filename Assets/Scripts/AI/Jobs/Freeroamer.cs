using UnityEngine;
public class Freeroamer : BaseJob, IJobRequirements, IConcreteJob
{
    private MapService.RuntimeMap runtimeEncroachMap;
    private MapService.RuntimeMap runtimeLeaveMap;
    private MapService.RuntimeMap currentMap;
    protected override BehaviourName InitialBehaviour => BehaviourName.ENCROACHING;
    private int minLeaveSteps = 3;
    private int currentLeaveStepsLeft;
    private DoorName lastFailedDoorLoc;
    private const float ReencroachChance = 0.6f;

    public Freeroamer(JobNecessities necessities, bool switchedFromAnotherJob = false)
        : base(necessities)
    {

        JobSpecificInitializations();

        if (necessities.LastLocation == RoomName.EAST_DOOR)
        {
            currentLoc = RoomName.EAST_HALL_FAR;
        }
        else if (necessities.LastLocation == RoomName.WEST_DOOR)
        {
            currentLoc = RoomName.WEST_HALL_FAR;
        }

        HandleSwitchInitialization(switchedFromAnotherJob);
        DeclareInitialization();
    }

    public void JobSpecificInitializations()
    {
        JobName = JobName.FREEROAMER;
        lastFailedDoorLoc = memories.lastFailedDoor;
        BuildMaps();
        MovementTimer = TimeGenerator.WeibullTime(AiLevel, profile);
        currentMap = runtimeEncroachMap;
    }

    public void HandleSwitchInitialization(bool switchedFromAnotherJob)
    {
        if (switchedFromAnotherJob)
        {
            currentMap = runtimeLeaveMap;
            SwitchToBehaviour(BehaviourName.LEAVING);
        }
    }

    public override void PerformJob()
    {
        switch (currentBehaviour)
        {
            case BehaviourName.ENCROACHING:
            case BehaviourName.LEAVING:
                PlayRedLightGreenLight();
                break;
            case BehaviourName.STALKING_DOOR:
                HandleKillPosition();
                break;
            default:
                return;
        }
    }

    public void PlayRedLightGreenLight()
    {
        if (activeCamera.room != currentLoc)
        {
            ReactToOthers();
            MovementTimer -= Time.deltaTime;

            if (MovementTimer <= 0f)
            {
                MovementTimer = TimeGenerator.WeibullTime(AiLevel, profile);
                StepandNotify();
            }
        }
    }


    public override RoomName CalculateNextPos()
    {
        var nextRoom = MapService.ChooseNext(currentMap.nodes[currentLoc]).roomName;

        if (currentBehaviour == BehaviourName.LEAVING)
        {
            if (currentLeaveStepsLeft > 0)
            {
                currentLeaveStepsLeft--;
            }
            else if (Random.value <= ReencroachChance)
            {
                SwitchToBehaviour(BehaviourName.ENCROACHING);
                nextRoom = MapService.ChooseNext(currentMap.nodes[currentLoc]).roomName;
            }
        }

        if (nextRoom == RoomName.EAST_DOOR || nextRoom == RoomName.WEST_DOOR)
            SwitchToBehaviour(BehaviourName.STALKING_DOOR);

        return nextRoom;
    }
    public override bool HandleNewBehaviour(BehaviourName newBehaviour)
    {
        switch (newBehaviour)
        {
            case BehaviourName.ENCROACHING:
                currentBehaviour = BehaviourName.ENCROACHING;
                MovementTimer = TimeGenerator.WeibullTime(AiLevel, profile);
                currentMap = runtimeEncroachMap;
                break;
            case BehaviourName.LEAVING:
                currentBehaviour = BehaviourName.LEAVING;
                MovementTimer = TimeGenerator.WeibullTime(AiLevel, profile);
                currentLeaveStepsLeft = minLeaveSteps;
                currentMap = runtimeLeaveMap;
                break;
            case BehaviourName.STALKING_DOOR:
                currentBehaviour = BehaviourName.STALKING_DOOR;
                MovementTimer = TimeGenerator.ExponentialTime(AiLevel, profile);
                break;
            default:
                return false;
        }

        return true;
    }

    public override void HandleKillPosition()
    {
        if (!AtADoor())
        {
            return;
        }

        MovementTimer -= Time.deltaTime;

        ReactToOthers();

        if (MovementTimer > 0f) return;

        else if (TheDoorImAtIsClosed())
        {
            if (currentLoc == RoomName.EAST_DOOR)
                lastFailedDoorLoc = DoorName.RIGHT;
            else if (currentLoc == RoomName.WEST_DOOR)
                lastFailedDoorLoc = DoorName.LEFT;
            else
                lastFailedDoorLoc = DoorName.NONE;

            memories.lastFailedDoor = lastFailedDoorLoc;

            if (AttemptToSwitchJobs())
                return;
            BuildMaps(lastFailedDoorLoc);
            SwitchToBehaviour(BehaviourName.LEAVING);
            StepandNotify();
        }
        else
        {
            KillPlayer();
        }
    }

    private void BuildMaps(DoorName doorFailed = DoorName.NONE)
    {
        runtimeEncroachMap = MapService.BuildRuntimeMap(mapGraphs.EncroachBaseMap, true, doorFailed);
        runtimeLeaveMap = MapService.BuildRuntimeMap(mapGraphs.LeaveBaseMap);
    }
}
