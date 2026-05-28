using System;
using UnityEngine;

public struct JobNecessities
{
    public GameContext gameContext;
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
    [SerializeField] private GameContext gameContext;
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
        if (gameContext == null || mapGraphs == null || profile == null)
        {
            Debug.LogError($"{name}: Missing references on Animatronic.");
        }

        recordedLoc = profile.nightStartingRoom;

        gameContext.RegisterAnimatronic(profile.characterName, this);
    }

    void OnEnable()
    {
        gameContext.AIMonitor.OnJobSwitched += HandleSwitchRequest;

        gameContext.AIMonitor.OnLocationChanged += HandleLocationChange;

        gameContext.AIMonitor.OnDeath += HandleDeath;

    }

    void OnDisable()
    {
        gameContext.AIMonitor.OnJobSwitched -= HandleSwitchRequest;

        gameContext.AIMonitor.OnLocationChanged -= HandleLocationChange;

        gameContext.AIMonitor.OnDeath -= HandleDeath;
    }
    void Start()
    {
        var defaultJob = GetDefaultJobForThisChar();

        memories = new AnimatronicMemory
        {
            lastFailedDoor = DoorName.NONE
        };

        necessities = new JobNecessities
        {
            gameContext = gameContext,
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

    public BehaviourName GetCurrentBehaviour()
    {
        if(myJob != null)
        {
            return myJob.currentBehaviour;
        }
        else
        {
            return BehaviourName.NONE;
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
            return CharacterName.NONE;
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
            return JobName.NONE;
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
        if (gameContext?.AIMonitor != null)
            gameContext.AIMonitor.OnJobSwitched -= HandleSwitchRequest;

        if (gameContext != null && profile != null)
            gameContext.UnregisterAnimatronic(profile.characterName, this);
    }
    
}
