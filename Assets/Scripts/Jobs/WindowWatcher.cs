using UnityEngine;
public class WindowWatcher : BaseJob, IJobRequirements, IConcreteJob
{
    protected override Behaviour InitialBehaviour => Behaviour.InvisibleRoaming;
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
            SwitchToBehaviour(Behaviour.InvisibleRoaming);
            currentLoc = LocName.NoWhere;
        }
    }

    public override void PerformJob()
    {
        switch (currentBehaviour)
        {
            case Behaviour.InvisibleRoaming:
                InvisibleRoam();
                break;
            case Behaviour.StareThroughWindow:
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
                        SwitchToBehaviour(Behaviour.InvisibleRoaming);
                        return LocName.NoWhere;
                    }

                    else
                    {
                        return door;
                    }
                }

            default:
                SwitchToBehaviour(Behaviour.InvisibleRoaming);
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
            SwitchToBehaviour(Behaviour.InvisibleRoaming);
            return chosenLocation;
        }

        else if (occupiedWest)
        {
            SwitchToBehaviour(Behaviour.StareThroughWindow);
            chosenLocation = LocName.EastDoor;
        }
        else if (occupiedEast)
        {
            SwitchToBehaviour(Behaviour.StareThroughWindow);
            chosenLocation = LocName.WestDoor;
        }
        else if (x <= 0.5f)
        {
            SwitchToBehaviour(Behaviour.StareThroughWindow);
            chosenLocation = LocName.WestDoor;
        }
        else
        {
            SwitchToBehaviour(Behaviour.StareThroughWindow);
            chosenLocation = LocName.EastDoor;
        }

        return chosenLocation;
    }

    private bool PlayerIsLookingAtThisWindow(LocName chosenLocation)
    {
        return (gameContext.Player.CurrentView == View.LeftDoor && chosenLocation == LocName.WestDoor) || (gameContext.Player.CurrentView == View.RightDoor && chosenLocation == LocName.EastDoor);
    }

    public override bool HandleNewBehaviour(Behaviour newBehaviour)
    {
        switch (newBehaviour)
        {
            case Behaviour.InvisibleRoaming:
                MovementTimer = TimeGenerator.WeibullTime(AiLevel);
                currentBehaviour = Behaviour.InvisibleRoaming;
                break;
            case Behaviour.StareThroughWindow:
                MovementTimer = TimeGenerator.WeibullTime(AiLevel, null, StaringTimeMultiplier);
                currentBehaviour = Behaviour.StareThroughWindow;
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
