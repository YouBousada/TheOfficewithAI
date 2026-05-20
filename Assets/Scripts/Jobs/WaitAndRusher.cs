using UnityEngine;
public class WaitAndRusher : BaseJob, IJobRequirements, IConcreteJob
{
    private MapService.RuntimeMap runtimeRunningMap;
    private BaseMap map;
    protected override Behaviour InitialBehaviour => Behaviour.Idling;
    private float maxIdleTime;
    private float maxRuntime;
    private DoorName lastFailedDoorLoc = DoorName.None;

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
        runtimeRunningMap = services.MapService.BuildRuntimeMap(map);
        HandleSwitchInitialization(switchedFromAnotherJob);
        DeclareInitialization();
    }

    public void JobSpecificInitializations()
    {
        JobName = JobName.WaitAndRusher;
        maxIdleTime = Mathf.Max(0.1f, profile.baseIdleTime - (AiLevelIdleScale * AiLevel) + (PatienceIdleScale * profile.patience));
        maxRuntime = Mathf.Lerp(MaxRunInterval, MinRunInterval, (AiLevel - 1f) / 19f);
        MovementTimer = maxIdleTime;
    }

    public void HandleSwitchInitialization(bool switchedFromAnotherJob)
    {
        if (switchedFromAnotherJob)
        {
            SwitchToBehaviour(Behaviour.Idling);
            currentLoc = PickRandomIdleRoom();
        }
    }

    public override void PerformJob()
    {
        switch (currentBehaviour)
        {
            case Behaviour.Idling:
                Wait();
                break;
            case Behaviour.Running:
                Run();
                break;
            case Behaviour.StalkDoor:
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
            newLoc = services.MapService.ChooseNext(runtimeRunningMap.nodes[currentLoc]).roomName;
        }

        return newLoc;
    }

    public override bool HandleNewBehaviour(Behaviour newBehaviour)
    {
        switch (newBehaviour)
        {
            case Behaviour.Running:
                MovementTimer = maxRuntime;
                currentBehaviour = Behaviour.Running;
                break;
            case Behaviour.Idling:
                MovementTimer = maxIdleTime;
                currentBehaviour = Behaviour.Idling;
                break;
            case Behaviour.StalkDoor:
                MovementTimer = StalkDoorInitialTimer;
                currentBehaviour = Behaviour.StalkDoor;
                break;
            default:
                return false;
        }
        return true;
    }

    void Wait()
    {
        if (world.CameraSystem.ActiveCamera == currentLoc)
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
            SwitchToBehaviour(Behaviour.Running);
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
            {
                lastFailedDoorLoc = DoorName.Right;
            }
            else if (currentLoc == LocName.WestDoor)
            {
                lastFailedDoorLoc = DoorName.Left;
            }
            else
            {
                lastFailedDoorLoc = DoorName.None;
            }
            if (AttemptToSwitchJobs())
                return;
            runtimeRunningMap = services.MapService.BuildRuntimeMap(map, false, lastFailedDoorLoc);
            SwitchToBehaviour(Behaviour.Idling);
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
            SwitchToBehaviour(Behaviour.StalkDoor);
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
