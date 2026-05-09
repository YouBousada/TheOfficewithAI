using UnityEngine;
public class WaitAndRusher : BaseJob, IJobRequirements, IConcreteJob
{
    private MapService.RuntimeMap RuntimeRunningMap;
    private BaseMap map;
    protected override Behaviour InitialBehaviour => Behaviour.Idling;
    private float maxIdleTime;
    private float maxRuntime;
    private DoorName lastFailedDoorLoc = DoorName.None;
    public WaitAndRusher(JobNecessities necessities, bool switchedFromAnotherJob = false)
        : base(necessities)
    {
        JobSpecificInitilizations();
        map = necessities.mapGraphs.runningBaseMap;
        RuntimeRunningMap = services.mapService.BuildRuntimeMap(map);
        HandleSwitchInitialization(switchedFromAnotherJob);
        DeclareInitilization();
    }

    public void JobSpecificInitilizations()
    {
        JobName = JobName.WaitAndRusher;
        maxIdleTime = 20f - (0.5f * AiLevel) + (0.5f * profile.patience);
        maxRuntime = Mathf.Lerp(3f, 2f, (AiLevel - 1f) / 19f);
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
            newLoc = services.mapService.ChooseNext(RuntimeRunningMap.nodes[currentLoc]).roomName;
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
                MovementTimer = 0.1f;
                currentBehaviour = Behaviour.StalkDoor;
                break;
            default:
                return false;
        }
        return true;
    }

    void Wait()
    {
        if (world.cameraSystem.activeCamera == currentLoc)
        {
            if (MovementTimer < maxIdleTime)
            {
                MovementTimer += 3 * Time.deltaTime;
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
            RuntimeRunningMap = services.mapService.BuildRuntimeMap(map, false, lastFailedDoorLoc);
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
