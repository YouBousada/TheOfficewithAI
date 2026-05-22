using System;
using UnityEngine;

public struct JobNecessities
{
    public GameContext world;
    public MapGraphs mapGraphs;
    public CharacterProfile givenProfile;
    public LocName LastLocation;
    public AnimatronicMemory memories;
    public int aiLevel;
}

public class AnimatronicMemory
{
    public DoorName lastFailedDoor;
}

public class Animatronic : MonoBehaviour
{
    [SerializeField] private GameContext world;
    [SerializeField] private MapGraphs mapGraphs;
    [SerializeField] private CharacterProfile profile;
    [SerializeField] private int Ailevel;

    private BaseJob myJob; 
    private LocName recordedLoc;
    private JobNecessities necessities;
    private AnimatronicMemory memories;
    private bool hasPendingSwitch;
    private JobName pendingJob;

    void Awake()
    {
        if (world == null || mapGraphs == null || profile == null)
            // Debug.LogError($"{name}: Missing references on Animatronic.");

        recordedLoc = profile.nightStartingRoom;

        world.RegisterAnimatronic(profile.characterName, this);
    }

    void OnEnable()
    {
        world.AIMonitor.OnJobSwitched += HandleSwitchRequest;

        world.AIMonitor.OnLocationChanged += HandleLocationChange;

        world.AIMonitor.OnDeath += HandleDeath;

    }

    void OnDisable()
    {
        world.AIMonitor.OnJobSwitched -= HandleSwitchRequest;

        world.AIMonitor.OnLocationChanged -= HandleLocationChange;

        world.AIMonitor.OnDeath -= HandleDeath;
    }
    void Start()
    {
        var defaultJob = GetDefaultJobForThisChar();

        memories = new AnimatronicMemory
        {
            lastFailedDoor = DoorName.None
        };

        necessities = new JobNecessities
        {
            world = world,
            mapGraphs = mapGraphs,
            givenProfile = profile,
            LastLocation = recordedLoc,
            memories = memories,
            aiLevel = Ailevel
        };
        
        if(myJob == null)
        {
            myJob = JobFactory.Create(defaultJob, necessities, false);
        }

    }
    private JobName GetDefaultJobForThisChar()
    {
        return profile.jobsByPreference[0];
    }

    public Behaviour GetCurrentBehaviour()
    {
        if(myJob != null)
        {
            return myJob.currentBehaviour;
        }
        else
        {
            return Behaviour.None;
        }
    }

    public CharacterName GetCharacterName()
    {
        if(profile != null)
        {
            return profile.characterName;
        }
        else
        {
            // Debug.LogWarning("Animatronic has no CharacterProfile assigned, cannot get CharacterName.");
            return CharacterName.None;
        }
    }

    public LocName GetCurrentLocation()
    {
        return recordedLoc;
    }

    public JobName GetCurrentJobName()
    {
        if(myJob != null)
        {
            return myJob.JobName;
        }
        else
        {
            // Debug.LogWarning("Animatronic has no current job assigned, cannot get JobName.");
            return JobName.None;
        }
    }

    void Update()
    {
        if(myJob != null)
        {
            myJob.PerformJob();
        }
    }

    void HandleSwitchRequest(CharacterName who, JobName fromJob, JobName toJob)
    {
        if (who != profile.characterName) return;
        if (hasPendingSwitch) return;
        pendingJob = toJob;
        hasPendingSwitch = true;
    }

    void HandleLocationChange(CharacterName who, LocName oldLocation, LocName newLocation)
    {
        if (who != profile.characterName) return;
        if(myJob != null)
        {
            recordedLoc = newLocation;
        }
    }

    void LateUpdate()
    {
        if (!hasPendingSwitch) return;
        hasPendingSwitch = false;

        LocName oldLocation = recordedLoc;
        necessities.LastLocation = oldLocation;
        myJob = JobFactory.Create(pendingJob, necessities, true);
        recordedLoc = myJob.currentLoc;
    }

    void HandleDeath(CharacterName who, LocName location)
    {
        if (who == profile.characterName){return;}
        
        if (myJob != null)
        {
            myJob = null;
        }
        
    }
    void OnDestroy()
    {
        if (world?.AIMonitor != null)
            world.AIMonitor.OnJobSwitched -= HandleSwitchRequest;

        if (world != null && profile != null)
            world.UnregisterAnimatronic(profile.characterName, this);
    }
    
}
