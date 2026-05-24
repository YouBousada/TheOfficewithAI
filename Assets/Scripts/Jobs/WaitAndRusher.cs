using UnityEngine;
public class WaitAndRusher : BaseJob, IJobRequirements, IConcreteJob
{
    private MapService.RuntimeMap runtimeRunningMap;
    private BaseMap map;
    protected override BehaviourName InitialBehaviour => BehaviourName.Idling;
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
        runtimeRunningMap = mapService.BuildRuntimeMap(map);
        HandleSwitchInitialization(switchedFromAnotherJob);
        DeclareInitialization();
    }

    public void JobSpecificInitializations()
    {
        JobName = JobName.WaitAndRusher;
        lastFailedDoorLoc = memories.lastFailedDoor;
        maxIdleTime = Mathf.Max(0.1f, profile.baseIdleTime - (AiLevelIdleScale * AiLevel) + (PatienceIdleScale * profile.patience));
        maxRuntime = Mathf.Lerp(MaxRunInterval, MinRunInterval, (AiLevel - 1f) / 19f);
        MovementTimer = maxIdleTime;
    }

    public void HandleSwitchInitialization(bool switchedFromAnotherJob)
    {
        if (switchedFromAnotherJob)
        {
            SwitchToBehaviour(BehaviourName.Idling);
            currentLoc = PickRandomIdleRoom();
        }
    }

    public override void PerformJob()
    {
        switch (currentBehaviour)
        {
            case BehaviourName.Idling:
                Wait();
                break;
            case BehaviourName.Running:
                Run();
                break;
            case BehaviourName.StalkDoor:
                HandleKillPosition();
                break;
            default:
                return;
        }
    }

    public override LocName CalculateNextPos()
    {
        LocName newLoc;

        if (currentLoc == LocName.WestDoor || currentLoc == LocName.EastDoor)
        {
            newLoc = PickRandomIdleRoom();
        }

        else
        {
            newLoc = mapService.ChooseNext(runtimeRunningMap.nodes[currentLoc]).roomName;
        }

        return newLoc;
    }

    public override bool HandleNewBehaviour(BehaviourName newBehaviour)
    {
        switch (newBehaviour)
        {
            case BehaviourName.Running:
                MovementTimer = maxRuntime;
                currentBehaviour = BehaviourName.Running;
                break;
            case BehaviourName.Idling:
                MovementTimer = maxIdleTime;
                currentBehaviour = BehaviourName.Idling;
                break;
            case BehaviourName.StalkDoor:
                MovementTimer = StalkDoorInitialTimer;
                currentBehaviour = BehaviourName.StalkDoor;
                break;
            default:
                return false;
        }
        return true;
    }

    void Wait()
    {
        if (gameContext.CameraSystem.ActiveCamera == currentLoc)
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
            SwitchToBehaviour(BehaviourName.Running);
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
            if (currentLoc == LocName.EastDoor)
                lastFailedDoorLoc = DoorName.Right;
            else if (currentLoc == LocName.WestDoor)
                lastFailedDoorLoc = DoorName.Left;
            else
                lastFailedDoorLoc = DoorName.None;

            memories.lastFailedDoor = lastFailedDoorLoc;

            if (AttemptToSwitchJobs())
                return;
            runtimeRunningMap = mapService.BuildRuntimeMap(map, false, lastFailedDoorLoc);
            SwitchToBehaviour(BehaviourName.Idling);
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
            SwitchToBehaviour(BehaviourName.StalkDoor);
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
