using UnityEngine;
public class Freeroamer : BaseJob, IJobRequirements, IConcreteJob
{
    private MapService.RuntimeMap RuntimeEncroachMap;
    private MapService.RuntimeMap RuntimeLeaveMap;
    private MapService.RuntimeMap currentMap;
    protected override Behaviour InitialBehaviour => Behaviour.Encroaching;
    private int minLeaveSteps = 3;
    private int currentLeaveStepsLeft;
    private DoorName lastFailedDoorLoc = DoorName.None;
    private const float ReencroachChance = 0.6f;




    public Freeroamer(JobNecessities necessities, bool switchedFromAnotherJob = false)
        : base(necessities)
    {

        JobSpecificInitilizations();

        if (necessities.lastlocation == LocName.EastDoor)
        {
            currentLoc = LocName.EastHall_Far;
        }
        else if (necessities.lastlocation == LocName.WestDoor)
        {
            currentLoc = LocName.WestHall_Far;
        }

        HandleSwitchInitialization(switchedFromAnotherJob);
        DeclareInitilization();
    }

    public void JobSpecificInitilizations()
    {
        JobName = JobName.Freeroamer;
        BuildMaps();
        MovementTimer = services.timeGenerator.WeibullTime(AiLevel, profile);
        currentMap = RuntimeEncroachMap;
    }

    public void HandleSwitchInitialization(bool switchedFromAnotherJob)
    {
        if (switchedFromAnotherJob)
        {
            currentMap = RuntimeLeaveMap;
            SwitchToBehaviour(Behaviour.Leaving);
        }
    }

    public override void PerformJob()
    {
        switch (currentBehaviour)
        {
            case Behaviour.Encroaching:
            case Behaviour.Leaving:
                PlayRedLightGreenLight();
                break;
            case Behaviour.StalkDoor:
                HandleKillPosition();
                break;
            default:
                return;
        }
    }

    public void PlayRedLightGreenLight()
    {
        if (world.cameraSystem.activeCamera != currentLoc)
        {
            ReactToOthers();
            MovementTimer -= Time.deltaTime;

            if (MovementTimer <= 0f)
            {
                MovementTimer = services.timeGenerator.WeibullTime(AiLevel, profile);
                StepandNotify();
            }
        }
    }


    public override LocName CalculateNextPos()
    {
        var nextRoom = services.mapService.ChooseNext(currentMap.nodes[currentLoc]).roomName;

        if (currentBehaviour == Behaviour.Leaving)
        {
            if (currentLeaveStepsLeft > 0)
            {
                currentLeaveStepsLeft--;
            }
            else if (Random.value <= ReencroachChance)
            {
                SwitchToBehaviour(Behaviour.Encroaching);
                nextRoom = services.mapService.ChooseNext(currentMap.nodes[currentLoc]).roomName;
            }
        }

        if (nextRoom == LocName.EastDoor || nextRoom == LocName.WestDoor)
            SwitchToBehaviour(Behaviour.StalkDoor);

        return nextRoom;
    }
    public override bool HandleNewBehaviour(Behaviour newBehaviour)
    {
        switch (newBehaviour)
        {
            case Behaviour.Encroaching:
                currentBehaviour = Behaviour.Encroaching;
                MovementTimer = services.timeGenerator.WeibullTime(AiLevel, profile);
                currentMap = RuntimeEncroachMap;
                break;
            case Behaviour.Leaving:
                currentBehaviour = Behaviour.Leaving;
                MovementTimer = services.timeGenerator.WeibullTime(AiLevel, profile);
                currentLeaveStepsLeft = minLeaveSteps;
                currentMap = RuntimeLeaveMap;
                break;
            case Behaviour.StalkDoor:
                currentBehaviour = Behaviour.StalkDoor;
                MovementTimer = services.timeGenerator.ExponentialTime(AiLevel, profile);
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
            Debug.LogWarning(profile.characterName + " tried to handle kill position while not at a door!");
            return;
        }

        MovementTimer -= Time.deltaTime;

        ReactToOthers();

        if (MovementTimer > 0f) return;

        else if (TheDoorImAtIsClosed())
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
            BuildMaps(lastFailedDoorLoc);
            SwitchToBehaviour(Behaviour.Leaving);
            StepandNotify();
        }
        else
        {
            KillPlayer();
        }
    }

    private void BuildMaps(DoorName doorFailed = DoorName.None)
    {
        RuntimeEncroachMap = services.mapService.BuildRuntimeMap(mapGraphs.encroachBaseMap, true, doorFailed);
        RuntimeLeaveMap = services.mapService.BuildRuntimeMap(mapGraphs.leaveBaseMap);
    }
}
