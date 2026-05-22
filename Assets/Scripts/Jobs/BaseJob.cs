using UnityEngine;

public abstract class BaseJob : IJobRequirements
{
    protected GameContext gameContext;
    protected MapGraphs mapGraphs;
    protected CharacterProfile profile;
    public LocName currentLoc;
    public Behaviour currentBehaviour;
    protected abstract Behaviour InitialBehaviour { get; }
    public JobName JobName;
    public AnimatronicMemory memories;
    protected int AiLevel;
    protected float MovementTimer;

    public BaseJob(JobNecessities necessities)
    {
        mapGraphs = necessities.mapGraphs;
        AiLevel = necessities.aiLevel;
        gameContext = necessities.gameContext;
        profile = necessities.givenProfile;
        currentLoc = necessities.LastLocation;
        memories = necessities.memories;
        currentBehaviour = InitialBehaviour;
    }
    public abstract void PerformJob();
    public abstract LocName CalculateNextPos();
    public abstract bool HandleNewBehaviour(Behaviour newBehaviour);
    public abstract void HandleKillPosition();


    public void DeclareInitialization()
    {
        gameContext.AIMonitor.HandleInitialization(profile.characterName, currentLoc, currentBehaviour, JobName);
    }
    public bool AtADoor()
    {
        return currentLoc == LocName.EastDoor || currentLoc == LocName.WestDoor;
    }
    public bool TheDoorImAtIsClosed()
    {
        if ((!gameContext.OfficeStates.LeftDoorIsOpen && currentLoc == LocName.WestDoor) || (!gameContext.OfficeStates.RightDoorIsOpen && currentLoc == LocName.EastDoor))
        {
            return true;
        }
        return false;
    }

    public void StepandNotify()
    {
        LocName oldlocation = currentLoc;
        LocName newLocation = CalculateNextPos();
        currentLoc = newLocation;
        gameContext.AIMonitor.HandleLocationChange(profile.characterName, oldlocation, currentLoc);
    }

    public void SwitchToBehaviour(Behaviour newBehaviour)
    {
        Behaviour oldBehaviour = currentBehaviour;
        bool flowControl = HandleNewBehaviour(newBehaviour);
        if (!flowControl)
        {
            return;
        }

        gameContext.AIMonitor.HandleBehaviourChange(profile.characterName, oldBehaviour, newBehaviour);
    }

    public void KillPlayer()
    {
        if (gameContext.Player.IsDead) return;
        gameContext.Player.Die();
        gameContext.AIMonitor.HandleDeath(profile.characterName, currentLoc);
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
            if (AskJobArbiterForJob(gameContext, profile.characterName, job))
                return true;
        }
        return false;
    }
    private const float JobSelectionChance = 0.8f;

    public bool AskJobArbiterForJob(GameContext gameContext, CharacterName character, JobName toJob)
    {
        if (JobArbiter.ApproveJobSwitch(gameContext, character, toJob))
        {
            float x = Random.value;
            if(x > JobSelectionChance)
            {
                return false;
            }
            gameContext.AIMonitor.HandleJobSwitch(character, JobName, toJob);
            return true;
        }
        return false;
    }

    public void RetreatFromDislikedCharacters()
    {
        if (profile.dislikedCharacters.Count == 0)
        {
            return;
        }
        foreach (CharacterName disliked in profile.dislikedCharacters)
            {
                if(gameContext.TryGetAnimatronic(disliked, out Animatronic dislikedAnim) && dislikedAnim.GetCurrentLocation() == currentLoc)
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
                if(gameContext.TryGetAnimatronic(liked, out Animatronic likedAnim) && likedAnim.GetCurrentLocation() == currentLoc)
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

    public bool OccupiedWindow(LocName windowLoc)
    {
        
        foreach (Animatronic anim in gameContext.Animatronics)
        {
            if (anim.GetCurrentLocation() == windowLoc && anim.GetCurrentBehaviour() == Behaviour.StareThroughWindow)
            {
                return true;
            }
        }
        return false;
    }

   

    

}
