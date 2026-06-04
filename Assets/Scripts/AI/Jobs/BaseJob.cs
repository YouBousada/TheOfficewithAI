using UnityEngine;

public abstract class BaseJob : IJobRequirements
{
    protected AiMonitor aiMonitor;
    protected Office officeStates;
    protected Player player;
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
    protected JobArbiter jobArbiter;

    public BaseJob(JobNecessities necessities)
    {
        mapGraphs = necessities.mapGraphs;
        AiLevel = necessities.aiLevel;
        aiMonitor           = necessities.aiMonitor;
        officeStates        = necessities.officeStates;
        player              = necessities.player;
        activeCamera        = necessities.activeCamera;
        animatronicRegistry = necessities.animatronicRegistry;
        profile = necessities.givenProfile;
        currentLoc = necessities.LastLocation;
        memories = necessities.memories;
        currentBehaviour = InitialBehaviour;
        jobArbiter = new JobArbiter(animatronicRegistry);
    }
    public abstract void PerformJob();
    public abstract RoomName CalculateNextPos();
    public abstract bool HandleNewBehaviour(BehaviourName newBehaviour);
    public abstract void HandleKillPosition();


    public void DeclareInitialization()
    {
        aiMonitor.HandleInitialization(profile.characterName, currentLoc, currentBehaviour, JobName);
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
        aiMonitor.HandleLocationChange(profile.characterName, oldlocation, currentLoc, currentBehaviour);
    }

    public void SwitchToBehaviour(BehaviourName newBehaviour)
    {
        BehaviourName oldBehaviour = currentBehaviour;
        bool flowControl = HandleNewBehaviour(newBehaviour);
        if (!flowControl)
        {
            return;
        }

        aiMonitor.HandleBehaviourChange(profile.characterName, oldBehaviour, newBehaviour, currentLoc);
    }

    public void KillPlayer()
    {
        if (player.IsDead) return;
        player.Die();
        aiMonitor.HandleDeath(profile.characterName, currentLoc);
    }

    public bool AttemptToSwitchJobs()
    {
        float x = Random.value;

        if (x > profile.willingnessToSwitchJobs)
        {
            return false;
        }

        foreach (var job in profile.jobsByPreference)
        {
            if (job == JobName) continue;
            if (AskJobArbiterForJob(profile.characterName, job))
                return true;
        }
        return false;
    }
    private const float JobSelectionChance = 0.8f;

    public bool AskJobArbiterForJob(CharacterName character, JobName toJob)
    {
        if (!jobArbiter.ApproveJobSwitch(character, toJob)) return false;

        if (Random.value > JobSelectionChance) return false;

        aiMonitor.HandleJobSwitch(character, JobName, toJob);
        return true;
    }

    public void RetreatFromDislikedCharacters()
    {
        if (profile.dislikedCharacters.Count == 0)
        {
            return;
        }
        foreach (CharacterName disliked in profile.dislikedCharacters)
            {
                if(animatronicRegistry.TryGetAnimatronic(disliked, out Animatronic dislikedAnim) && dislikedAnim.GetCurrentLocation() == currentLoc)
                {
                    MovementTimer -= Time.deltaTime * profile.socialRepelStrength;
                }

            }
    }

    public void StayWithLikedCharacters()
    {
        if (profile.likedCharacters.Count == 0)
        {
            return;
        }
        foreach (CharacterName liked in profile.likedCharacters)
            {
                if(animatronicRegistry.TryGetAnimatronic(liked, out Animatronic likedAnim) && likedAnim.GetCurrentLocation() == currentLoc)
                {
                    MovementTimer += Time.deltaTime * profile.socialAttractStrength;
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
