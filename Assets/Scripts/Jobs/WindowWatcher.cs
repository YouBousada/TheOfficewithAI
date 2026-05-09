using UnityEngine;
public class WindowWatcher : BaseJob, IJobRequirements, IConcreteJob
{
    protected override Behaviour InitialBehaviour => Behaviour.InvisableRoaming;

    public WindowWatcher(JobNecessities necessities, bool switchedFromAnotherJob = false)
        : base(necessities)
    {
        JobSpecificInitilizations();
        HandleSwitchInitialization(switchedFromAnotherJob);
        DeclareInitilization();

    }

    public void JobSpecificInitilizations()
    {
        JobName = JobName.WindowWatcher;
        MovementTimer = services.timeGenerator.WeibullTime(AiLevel, profile);
    }

    public void HandleSwitchInitialization(bool switchedFromAnotherJob)
    {
        if (switchedFromAnotherJob)
        {
            SwitchToBehaviour(Behaviour.InvisableRoaming);
            currentLoc = LocName.NoWhere;
        }
    }

    public override void PerformJob()
    {
        switch (currentBehaviour)
        {
            case Behaviour.InvisableRoaming:
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
            MovementTimer = services.timeGenerator.WeibullTime(AiLevel, profile);
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
                        SwitchToBehaviour(Behaviour.InvisableRoaming);
                        return LocName.NoWhere;
                    }

                    else
                    {
                        return door;
                    }
                }

            default:
                SwitchToBehaviour(Behaviour.InvisableRoaming);
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
            SwitchToBehaviour(Behaviour.InvisableRoaming);
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
        return (world.player.currentView == View.LeftDoor && chosenLocation == LocName.WestDoor) || (world.player.currentView == View.RightDoor && chosenLocation == LocName.EastDoor);
    }

    public override bool HandleNewBehaviour(Behaviour newBehaviour)
    {
        switch (newBehaviour)
        {
            case Behaviour.InvisableRoaming:
                MovementTimer = services.timeGenerator.WeibullTime(AiLevel);
                currentBehaviour = Behaviour.InvisableRoaming;
                break;
            case Behaviour.StareThroughWindow:
                MovementTimer = services.timeGenerator.WeibullTime(AiLevel, null, 0.5f);
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
            Debug.LogWarning(profile.characterName + " tried to handle kill position while not at a door!");
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
            MovementTimer = services.timeGenerator.ExponentialTime(AiLevel);
            StepandNotify();
        }
        else
        {
            KillPlayer();
        }
    }

    public bool PlayerIsLookingAtMeThroughWindow()
    {
        if (world.player.currentView == View.LeftDoor && currentLoc == LocName.WestDoor || world.player.currentView == View.RightDoor && currentLoc == LocName.EastDoor)
        {
            return true;
        }
        return false;
    }
}
