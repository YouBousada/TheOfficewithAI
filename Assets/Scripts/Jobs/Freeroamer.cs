using UnityEngine;
public class Freeroamer : BaseJob, IJobRequirements, IConcreteJob
{
    private MapService.RuntimeMap runtimeEncroachMap;
    private MapService.RuntimeMap runtimeLeaveMap;
    private MapService.RuntimeMap currentMap;
    protected override BehaviourName InitialBehaviour => BehaviourName.Encroaching;
    private int minLeaveSteps = 3;
    private int currentLeaveStepsLeft;
    private DoorName lastFailedDoorLoc;
    private const float ReencroachChance = 0.6f;

    public Freeroamer(JobNecessities necessities, bool switchedFromAnotherJob = false)
        : base(necessities)
    {

        JobSpecificInitializations();

        if (necessities.LastLocation == LocName.EastDoor)
        {
            currentLoc = LocName.EastHall_Far;
        }
        else if (necessities.LastLocation == LocName.WestDoor)
        {
            currentLoc = LocName.WestHall_Far;
        }

        HandleSwitchInitialization(switchedFromAnotherJob);
        DeclareInitialization();
    }

    public void JobSpecificInitializations()
    {
        JobName = JobName.Freeroamer;
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
            SwitchToBehaviour(BehaviourName.Leaving);
        }
    }

    public override void PerformJob()
    {
        switch (currentBehaviour)
        {
            case BehaviourName.Encroaching:
            case BehaviourName.Leaving:
                PlayRedLightGreenLight();
                break;
            case BehaviourName.StalkDoor:
                HandleKillPosition();
                break;
            default:
                return;
        }
    }

    public void PlayRedLightGreenLight()
    {
        if (gameContext.CameraSystem.ActiveCamera != currentLoc)
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


    public override LocName CalculateNextPos()
    {
        var nextRoom = MapService.ChooseNext(currentMap.nodes[currentLoc]).roomName;

        if (currentBehaviour == BehaviourName.Leaving)
        {
            if (currentLeaveStepsLeft > 0)
            {
                currentLeaveStepsLeft--;
            }
            else if (Random.value <= ReencroachChance)
            {
                SwitchToBehaviour(BehaviourName.Encroaching);
                nextRoom = MapService.ChooseNext(currentMap.nodes[currentLoc]).roomName;
            }
        }

        if (nextRoom == LocName.EastDoor || nextRoom == LocName.WestDoor)
            SwitchToBehaviour(BehaviourName.StalkDoor);

        return nextRoom;
    }
    public override bool HandleNewBehaviour(BehaviourName newBehaviour)
    {
        switch (newBehaviour)
        {
            case BehaviourName.Encroaching:
                currentBehaviour = BehaviourName.Encroaching;
                MovementTimer = TimeGenerator.WeibullTime(AiLevel, profile);
                currentMap = runtimeEncroachMap;
                break;
            case BehaviourName.Leaving:
                currentBehaviour = BehaviourName.Leaving;
                MovementTimer = TimeGenerator.WeibullTime(AiLevel, profile);
                currentLeaveStepsLeft = minLeaveSteps;
                currentMap = runtimeLeaveMap;
                break;
            case BehaviourName.StalkDoor:
                currentBehaviour = BehaviourName.StalkDoor;
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
            // Debug.LogWarning(profile.characterName + " tried to handle kill position while not at a door!");
            return;
        }

        MovementTimer -= Time.deltaTime;

        ReactToOthers();

        if (MovementTimer > 0f) return;

        else if (TheDoorImAtIsClosed())
        {
            if (currentLoc == LocName.EastDoor)
                lastFailedDoorLoc = DoorName.Right;
            else if (currentLoc == LocName.WestDoor)
                lastFailedDoorLoc = DoorName.Left;
            else
                lastFailedDoorLoc = DoorName.None;

            memories.lastFailedDoor = lastFailedDoorLoc;

            if (AttemptToSwitchJobs())
                return;
            BuildMaps(lastFailedDoorLoc);
            SwitchToBehaviour(BehaviourName.Leaving);
            StepandNotify();
        }
        else
        {
            KillPlayer();
        }
    }

    private void BuildMaps(DoorName doorFailed = DoorName.None)
    {
        runtimeEncroachMap = MapService.BuildRuntimeMap(mapGraphs.EncroachBaseMap, true, doorFailed);
        runtimeLeaveMap = MapService.BuildRuntimeMap(mapGraphs.LeaveBaseMap);
    }
}
