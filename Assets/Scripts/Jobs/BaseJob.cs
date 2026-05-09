using UnityEngine;

public abstract class BaseJob : IJobRequirements
{
    protected World world;
    protected Services services;
    protected MapGraphs mapGraphs;
    protected CharacterProfile profile;
    public LocName currentLoc;
    public Behaviour currentBehaviour;
    protected abstract Behaviour InitialBehaviour { get; }
    public JobName JobName;
    protected int AiLevel;
    protected float MovementTimer;

    public BaseJob(JobNecessities necessities)
    {
        mapGraphs = necessities.mapGraphs;
        services = necessities.services;
        AiLevel = necessities.aiLevel;
        world = necessities.world;
        profile = necessities.givenProfile;
        currentLoc = necessities.lastlocation;
        currentBehaviour = InitialBehaviour;
    }
    public abstract void PerformJob();
    public abstract LocName CalculateNextPos();
    public abstract bool HandleNewBehaviour(Behaviour newBehaviour);
    public abstract void HandleKillPosition();


    public void DeclareInitilization()
    {
        world.aiMonitor.HandleInitialization(profile.characterName, currentLoc, currentBehaviour, JobName);
    }
    public bool AtADoor()
    {
        return currentLoc == LocName.EastDoor || currentLoc == LocName.WestDoor;
    }
    public bool TheDoorImAtIsClosed()
    {
        if ((!world.leftDoorIsOpen && currentLoc == LocName.WestDoor) || (!world.rightDoorIsOpen && currentLoc == LocName.EastDoor))
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
        world.aiMonitor.HandleLocationChange(profile.characterName, oldlocation, currentLoc);
    }

    public void SwitchToBehaviour(Behaviour newBehaviour)
    {
        Behaviour oldBehaviour = currentBehaviour;
        bool flowControl = HandleNewBehaviour(newBehaviour);
        if (!flowControl)
        {
            return;
        }

        world.aiMonitor.HandleBehaviourChange(profile.characterName, oldBehaviour, newBehaviour);
    }

    public void KillPlayer()
    {
        if (world.player.isDead)
        { 
        return;
        }
        
        world.player.isDead = true;
        world.aiMonitor.HandleDeath(profile.characterName, currentLoc);
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
            if (AskForJob(services.jobArbiter, world, profile.characterName, job))
                return true;
        }
        return false;
    }
    public bool AskForJob(JobArbiter arbiter, World world, CharacterName character, JobName toJob)
    {
        if (arbiter.ApproveJobSwitch(world, character, toJob))
        {
            float x = Random.value;
            if(x > 0.8f)
            {
                return false;
            }
            world.aiMonitor.HandleJobSwitch(character, JobName, toJob);
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
                if(world.animatronicSet[disliked].GetCurrentLocation() == currentLoc)
                {
                    MovementTimer -= Time.deltaTime * 10f;
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
                if(world.animatronicSet[liked].GetCurrentLocation() == currentLoc)
                {
                    MovementTimer += Time.deltaTime * 0.5f;
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
        for (int i = 0; i < world.animatronicList.Count; i++)
        {
            var anim = world.animatronicList[i];
            if (anim.GetCurrentLocation() == windowLoc && anim.GetCurrentBehaviour() == Behaviour.StareThroughWindow)
            {
                return true;
            }
        }
        return false;
    }

   

    

}
