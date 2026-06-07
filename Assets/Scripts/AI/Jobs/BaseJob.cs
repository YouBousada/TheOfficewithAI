using UnityEngine;

public abstract class BaseJob : IJobRequirements
{
    protected AiEvents aiEvents;
    protected Office officeStates;
    protected CurrentView currentPlayerView;
    protected ActiveCameraVariable activeCamera;
    protected AnimatronicRegistry animatronicRegistry;
    protected MapGraphs mapGraphs;
    protected CharacterProfile profile;
    public RoomName currentLoc;
    public BehaviourName currentBehaviour;
    protected abstract BehaviourName InitialBehaviour { get; }
    public JobName JobName;
    public AnimatronicMemory memories;
    protected int AiLevel;
    protected float MovementTimer;
    public BaseJob(JobNecessities necessities)
    {
        mapGraphs           = necessities.mapGraphs;
        AiLevel             = necessities.aiLevel;
        currentPlayerView   = necessities.currentPlayerView;
        aiEvents            = necessities.aiEvents;
        officeStates        = necessities.officeStates;
        activeCamera        = necessities.activeCamera;
        animatronicRegistry = necessities.animatronicRegistry;
        profile             = necessities.givenProfile;
        currentLoc          = necessities.LastLocation;
        memories            = necessities.memories;
        currentBehaviour    = InitialBehaviour;
    }
    public abstract void PerformJob();
    public abstract RoomName CalculateNextPos();
    public abstract bool HandleNewBehaviour(BehaviourName newBehaviour);
    public abstract void HandleKillPosition();


    public void DeclareInitialization()
    {
        aiEvents.OnAnimatronicInitialize.Raise(new InitializationData { who = profile.CharacterName, location = currentLoc, behaviour = currentBehaviour, job = JobName });
    }
    public bool AtADoor()
    {
        return currentLoc == RoomName.EAST_DOOR || currentLoc == RoomName.WEST_DOOR;
    }
    public bool TheDoorImAtIsClosed()
    {
        if ((!officeStates.LeftDoorIsOpen && currentLoc == RoomName.WEST_DOOR) || (!officeStates.RightDoorIsOpen && currentLoc == RoomName.EAST_DOOR))
        {
            return true;
        }
        return false;
    }

    public void StepandNotify()
    {
        RoomName oldlocation = currentLoc;
        RoomName newLocation = CalculateNextPos();
        currentLoc = newLocation;
        aiEvents.OnLocationChanged.Raise(new LocationChangedData { who = profile.CharacterName, from = oldlocation, to = currentLoc, behaviour = currentBehaviour });
    }

    public void SwitchToBehaviour(BehaviourName newBehaviour)
    {
        BehaviourName oldBehaviour = currentBehaviour;
        bool flowControl = HandleNewBehaviour(newBehaviour);
        if (!flowControl)
        {
            return;
        }

        aiEvents.OnBehaviourChanged.Raise(new BehaviourChangedData { who = profile.CharacterName, oldBehaviour = oldBehaviour, newBehaviour = newBehaviour, location = currentLoc });
    }

    public void InitiateJumpscareEvent()
    {
        aiEvents.OnJumpscare.Raise(new JumpscareEventData { who = profile.CharacterName, from = currentLoc });
    }

    public bool AttemptToSwitchJobs()
    {
        float x = Random.value;

        if (x > profile.WillingnessToSwitchJobs)
        {
            return false;
        }

        foreach (var job in profile.JobsByPreference)
        {
            if (job == JobName) continue;
            if (AskJobArbiterForJob(profile.CharacterName, job))
                return true;
        }
        return false;
    }
    private const float JobSelectionChance = 0.8f;

    public bool AskJobArbiterForJob(CharacterName character, JobName toJob)
    {
        if (!JobArbiter.ApproveJobSwitch(animatronicRegistry, character, toJob)) return false;

        if (Random.value > JobSelectionChance) return false;
        aiEvents.OnJobSwitched.Raise(new JobSwitchedData { who = character, fromJob = JobName, toJob = toJob });
        return true;
    }

    public void RetreatFromDislikedCharacters()
    {
        if (profile.DislikedCharacters.Count == 0)
        {
            return;
        }
        foreach (CharacterName disliked in profile.DislikedCharacters)
            {
                if(animatronicRegistry.TryGetAnimatronic(disliked, out Animatronic dislikedAnim) && dislikedAnim.GetCurrentLocation() == currentLoc)
                {
                    MovementTimer -= Time.deltaTime * profile.SocialRepelStrength;
                }

            }
    }

    public void StayWithLikedCharacters()
    {
        if (profile.LikedCharacters.Count == 0)
        {
            return;
        }
        foreach (CharacterName liked in profile.LikedCharacters)
            {
                if(animatronicRegistry.TryGetAnimatronic(liked, out Animatronic likedAnim) && likedAnim.GetCurrentLocation() == currentLoc)
                {
                    MovementTimer += Time.deltaTime * profile.SocialAttractStrength;
                }

            }
    }

    public void ReactToOthers()
    {
        RetreatFromDislikedCharacters();
        StayWithLikedCharacters();
    }

    public bool OccupiedWindow(RoomName windowLoc)
    {
        
        foreach (Animatronic anim in animatronicRegistry.Animatronics)
        {
            if (anim.GetCurrentLocation() == windowLoc && anim.GetCurrentBehaviour() == BehaviourName.STARING_THROUGH_WINDOW)
            {
                return true;
            }
        }
        return false;
    }

   

    

}
