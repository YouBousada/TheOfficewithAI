using UnityEngine;
public class WindowWatcher : BaseJob, IJobRequirements, IConcreteJob
{
    protected override BehaviourName InitialBehaviour => BehaviourName.InvisibleRoaming;
    private const float StaringTimeMultiplier = 0.5f;

    public WindowWatcher(JobNecessities necessities, bool switchedFromAnotherJob = false)
        : base(necessities)
    {
        JobSpecificInitializations();
        HandleSwitchInitialization(switchedFromAnotherJob);
        DeclareInitialization();

    }

    public void JobSpecificInitializations()
    {
        JobName = JobName.WindowWatcher;
        MovementTimer = TimeGenerator.WeibullTime(AiLevel, profile);
    }

    public void HandleSwitchInitialization(bool switchedFromAnotherJob)
    {
        if (switchedFromAnotherJob)
        {
            SwitchToBehaviour(BehaviourName.InvisibleRoaming);
            currentLoc = LocName.NoWhere;
        }
    }

    public override void PerformJob()
    {
        switch (currentBehaviour)
        {
            case BehaviourName.InvisibleRoaming:
                InvisibleRoam();
                break;
            case BehaviourName.StareThroughWindow:
                HandleKillPosition();
                break;
            default:
                return;
        }
    }

    void InvisibleRoam()
    {
        MovementTimer -= Time.deltaTime;
        if (MovementTimer <= 0f)
        {
            MovementTimer = TimeGenerator.WeibullTime(AiLevel, profile);
            StepandNotify();
        }
    }

    public override LocName CalculateNextPos()
    {
        switch (currentLoc)
        {
            case LocName.NoWhere:

                {
                    LocName door = ChooseDoor();

                    if (PlayerIsLookingAtThisWindow(door))
                    {
                        SwitchToBehaviour(BehaviourName.InvisibleRoaming);
                        return LocName.NoWhere;
                    }

                    else
                    {
                        return door;
                    }
                }

            default:
                SwitchToBehaviour(BehaviourName.InvisibleRoaming);
                return LocName.NoWhere;
        }
    }

    private LocName ChooseDoor()
    {
        float x = Random.value;
        bool occupiedWest = OccupiedWindow(LocName.WestDoor);
        bool occupiedEast = OccupiedWindow(LocName.EastDoor);

        LocName chosenLocation = LocName.NoWhere;

        if (occupiedEast && occupiedWest)
        {
            SwitchToBehaviour(BehaviourName.InvisibleRoaming);
            return chosenLocation;
        }

        else if (occupiedWest)
        {
            SwitchToBehaviour(BehaviourName.StareThroughWindow);
            chosenLocation = LocName.EastDoor;
        }
        else if (occupiedEast)
        {
            SwitchToBehaviour(BehaviourName.StareThroughWindow);
            chosenLocation = LocName.WestDoor;
        }
        else if (x <= 0.5f)
        {
            SwitchToBehaviour(BehaviourName.StareThroughWindow);
            chosenLocation = LocName.WestDoor;
        }
        else
        {
            SwitchToBehaviour(BehaviourName.StareThroughWindow);
            chosenLocation = LocName.EastDoor;
        }

        return chosenLocation;
    }

    private bool PlayerIsLookingAtThisWindow(LocName chosenLocation)
    {
        return (gameContext.Player.CurrentView == View.LeftDoor && chosenLocation == LocName.WestDoor) || (gameContext.Player.CurrentView == View.RightDoor && chosenLocation == LocName.EastDoor);
    }

    public override bool HandleNewBehaviour(BehaviourName newBehaviour)
    {
        switch (newBehaviour)
        {
            case BehaviourName.InvisibleRoaming:
                MovementTimer = TimeGenerator.WeibullTime(AiLevel);
                currentBehaviour = BehaviourName.InvisibleRoaming;
                break;
            case BehaviourName.StareThroughWindow:
                MovementTimer = TimeGenerator.WeibullTime(AiLevel, null, StaringTimeMultiplier);
                currentBehaviour = BehaviourName.StareThroughWindow;
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

        if (PlayerIsLookingAtMeThroughWindow())
        {
            return;
        }

        MovementTimer -= Time.deltaTime;

        if (MovementTimer > 0f) return;

        if (TheDoorImAtIsClosed())
        {
            if (AttemptToSwitchJobs())
                return;
            MovementTimer = TimeGenerator.ExponentialTime(AiLevel);
            StepandNotify();
        }
        else
        {
            KillPlayer();
        }
    }

    public bool PlayerIsLookingAtMeThroughWindow()
    {
        if (gameContext.Player.CurrentView == View.LeftDoor && currentLoc == LocName.WestDoor || gameContext.Player.CurrentView == View.RightDoor && currentLoc == LocName.EastDoor)
        {
            return true;
        }
        return false;
    }
}
