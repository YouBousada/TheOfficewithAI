using UnityEngine;
public class WaitAndRusher : BaseJob, IJobRequirements, IConcreteJob
{
    private MapService.RuntimeMap runtimeRunningMap;
    private BaseMap map;
    protected override BehaviourName InitialBehaviour => BehaviourName.IDLING;
    private float maxIdleTime;
    private float maxRuntime;
    private DoorName lastFailedDoorLoc;

    private const float AiLevelIdleScale = 0.5f;
    private const float PatienceIdleScale = 0.5f;
    private const float MaxRunInterval = 3f;
    private const float MinRunInterval = 2f;
    private const float StalkDoorInitialTimer = 0.1f;
    private const float CameraWatchedRecoveryRate = 3f;
    public WaitAndRusher(JobNecessities necessities, bool switchedFromAnotherJob = false)
        : base(necessities)
    {
        JobSpecificInitializations();
        map = necessities.mapGraphs.RunningBaseMap;
        runtimeRunningMap = MapService.BuildRuntimeMap(map);
        HandleSwitchInitialization(switchedFromAnotherJob);
        DeclareInitialization();
    }

    public void JobSpecificInitializations()
    {
        JobName = JobName.WAIT_AND_RUSHER;
        lastFailedDoorLoc = memories.lastFailedDoor;
        maxIdleTime = Mathf.Max(0.1f, profile.baseIdleTime - (AiLevelIdleScale * AiLevel) + (PatienceIdleScale * profile.patience));
        maxRuntime = Mathf.Lerp(MaxRunInterval, MinRunInterval, (AiLevel - 1f) / 19f);
        MovementTimer = maxIdleTime;
    }

    public void HandleSwitchInitialization(bool switchedFromAnotherJob)
    {
        if (switchedFromAnotherJob)
        {
            SwitchToBehaviour(BehaviourName.IDLING);
            currentLoc = PickRandomIdleRoom();
        }
    }

    public override void PerformJob()
    {
        switch (currentBehaviour)
        {
            case BehaviourName.IDLING:
                Wait();
                break;
            case BehaviourName.RUNNING:
                Run();
                break;
            case BehaviourName.STALKING_DOOR:
                HandleKillPosition();
                break;
            default:
                return;
        }
    }

    public override LocName CalculateNextPos()
    {
        LocName newLoc;

        if (currentLoc == LocName.WEST_DOOR || currentLoc == LocName.EAST_DOOR)
        {
            newLoc = PickRandomIdleRoom();
        }

        else
        {
            newLoc = MapService.ChooseNext(runtimeRunningMap.nodes[currentLoc]).roomName;
        }

        return newLoc;
    }

    public override bool HandleNewBehaviour(BehaviourName newBehaviour)
    {
        switch (newBehaviour)
        {
            case BehaviourName.RUNNING:
                MovementTimer = maxRuntime;
                currentBehaviour = BehaviourName.RUNNING;
                break;
            case BehaviourName.IDLING:
                MovementTimer = maxIdleTime;
                currentBehaviour = BehaviourName.IDLING;
                break;
            case BehaviourName.STALKING_DOOR:
                MovementTimer = StalkDoorInitialTimer;
                currentBehaviour = BehaviourName.STALKING_DOOR;
                break;
            default:
                return false;
        }
        return true;
    }

    void Wait()
    {
        if (cameraSystem.ActiveCamera == currentLoc)
        {
            if (MovementTimer < maxIdleTime)
            {
                MovementTimer += CameraWatchedRecoveryRate * Time.deltaTime;
            }
        }
        else
        {
            ReactToOthers();
            MovementTimer -= Time.deltaTime;
        }

        if (MovementTimer <= 0f)
        {
            SwitchToBehaviour(BehaviourName.RUNNING);
        }
    }

    public override void HandleKillPosition()
    {
        if (MovementTimer > 0f)
        {
            MovementTimer -= Time.deltaTime;
        }
        else if (!TheDoorImAtIsClosed())
        {
            KillPlayer();
        }
        else
        {
            if (currentLoc == LocName.EAST_DOOR)
                lastFailedDoorLoc = DoorName.RIGHT;
            else if (currentLoc == LocName.WEST_DOOR)
                lastFailedDoorLoc = DoorName.LEFT;
            else
                lastFailedDoorLoc = DoorName.NONE;

            memories.lastFailedDoor = lastFailedDoorLoc;

            if (AttemptToSwitchJobs())
                return;
            runtimeRunningMap = MapService.BuildRuntimeMap(map, false, lastFailedDoorLoc);
            SwitchToBehaviour(BehaviourName.IDLING);
            StepandNotify();
        }
    }

    private LocName PickRandomIdleRoom()
    {
        return profile.possibleIdleRooms[Random.Range(0, profile.possibleIdleRooms.Count)];
    }

    private void Run()
    {
        if (AtADoor())
        {
            SwitchToBehaviour(BehaviourName.STALKING_DOOR);
            return;
        }

        MovementTimer -= Time.deltaTime;
        if (MovementTimer <= 0f)
        {
            StepandNotify();
            MovementTimer = maxRuntime;
        }
    }
}
