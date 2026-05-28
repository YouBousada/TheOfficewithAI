using UnityEngine;
public class WindowWatcher : BaseJob, IJobRequirements, IConcreteJob
{
    protected override BehaviourName InitialBehaviour => BehaviourName.INVISIBLE_ROAMING;
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
        JobName = JobName.WINDOW_WATCHER;
        MovementTimer = timeGenerator.WeibullTime(AiLevel, profile);
    }

    public void HandleSwitchInitialization(bool switchedFromAnotherJob)
    {
        if (switchedFromAnotherJob)
        {
            SwitchToBehaviour(BehaviourName.INVISIBLE_ROAMING);
            currentLoc = LocName.NOWHERE;
        }
    }

    public override void PerformJob()
    {
        switch (currentBehaviour)
        {
            case BehaviourName.INVISIBLE_ROAMING:
                InvisibleRoam();
                break;
            case BehaviourName.STARING_THROUGH_WINDOW:
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
            MovementTimer = timeGenerator.WeibullTime(AiLevel, profile);
            StepandNotify();
        }
    }

    public override LocName CalculateNextPos()
    {
        switch (currentLoc)
        {
            case LocName.NOWHERE:

                {
                    LocName door = ChooseDoor();

                    if (PlayerIsLookingAtThisWindow(door))
                    {
                        SwitchToBehaviour(BehaviourName.INVISIBLE_ROAMING);
                        return LocName.NOWHERE;
                    }

                    else
                    {
                        return door;
                    }
                }

            default:
                SwitchToBehaviour(BehaviourName.INVISIBLE_ROAMING);
                return LocName.NOWHERE;
        }
    }

    private LocName ChooseDoor()
    {
        float x = Random.value;
        bool occupiedWest = OccupiedWindow(LocName.WEST_DOOR);
        bool occupiedEast = OccupiedWindow(LocName.EAST_DOOR);

        LocName chosenLocation = LocName.NOWHERE;

        if (occupiedEast && occupiedWest)
        {
            SwitchToBehaviour(BehaviourName.INVISIBLE_ROAMING);
            return chosenLocation;
        }

        else if (occupiedWest)
        {
            SwitchToBehaviour(BehaviourName.STARING_THROUGH_WINDOW);
            chosenLocation = LocName.EAST_DOOR;
        }
        else if (occupiedEast)
        {
            SwitchToBehaviour(BehaviourName.STARING_THROUGH_WINDOW);
            chosenLocation = LocName.WEST_DOOR;
        }
        else if (x <= 0.5f)
        {
            SwitchToBehaviour(BehaviourName.STARING_THROUGH_WINDOW);
            chosenLocation = LocName.WEST_DOOR;
        }
        else
        {
            SwitchToBehaviour(BehaviourName.STARING_THROUGH_WINDOW);
            chosenLocation = LocName.EAST_DOOR;
        }

        return chosenLocation;
    }

    private bool PlayerIsLookingAtThisWindow(LocName chosenLocation)
    {
        return (player.CurrentView == ViewName.LEFT_DOOR && chosenLocation == LocName.WEST_DOOR) || (player.CurrentView == ViewName.RIGHT_DOOR && chosenLocation == LocName.EAST_DOOR);
    }

    public override bool HandleNewBehaviour(BehaviourName newBehaviour)
    {
        switch (newBehaviour)
        {
            case BehaviourName.INVISIBLE_ROAMING:
                MovementTimer = timeGenerator.WeibullTime(AiLevel);
                currentBehaviour = BehaviourName.INVISIBLE_ROAMING;
                break;
            case BehaviourName.STARING_THROUGH_WINDOW:
                MovementTimer = timeGenerator.WeibullTime(AiLevel, null, StaringTimeMultiplier);
                currentBehaviour = BehaviourName.STARING_THROUGH_WINDOW;
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
            MovementTimer = timeGenerator.ExponentialTime(AiLevel);
            StepandNotify();
        }
        else
        {
            KillPlayer();
        }
    }

    public bool PlayerIsLookingAtMeThroughWindow()
    {
        if (player.CurrentView == ViewName.LEFT_DOOR && currentLoc == LocName.WEST_DOOR || player.CurrentView == ViewName.RIGHT_DOOR && currentLoc == LocName.EAST_DOOR)
        {
            return true;
        }
        return false;
    }
}
