using UnityEngine;

public class Animatronic : MonoBehaviour
{
    [SerializeField] private World world;
    [SerializeField] private Services services;
    [SerializeField] private MapGraphs mapGraphs;
    [SerializeField] private CharacterProfile profile;
    [SerializeField] private int Ailevel;

    private BaseJob myJob; 
    private LocName recordedLoc;
    private JobNecessities necessities;
    private bool hasPendingSwitch;
    private JobName pendingJob;

    void Awake()
    {
        if (world == null || services == null || mapGraphs == null || profile == null)
            Debug.LogError($"{name}: Missing references on Animatronic.");

        recordedLoc = profile.nightStartingRoom;

        world.animatronicDictionary.Add(profile.characterName, this);
    }

    void OnEnable()
    {
        world.aiMonitor.OnJobSwitched += HandleSwitchRequest;

        world.aiMonitor.OnLocationChanged += HandleLocationChange;

        world.aiMonitor.OnDeath += HandleDeath;

    }

    void OnDisable()
    {
        world.aiMonitor.OnJobSwitched -= HandleSwitchRequest;

        world.aiMonitor.OnLocationChanged -= HandleLocationChange;

        world.aiMonitor.OnDeath -= HandleDeath;
    }
    void Start()
    {
        var defaultJob = GetDefaultJobForThisChar();

        necessities = new JobNecessities
        {
            world = world,
            services = services,
            mapGraphs = mapGraphs,
            givenProfile = profile,
            lastlocation = recordedLoc,
            aiLevel = Ailevel
        };
        
        if(myJob == null)
        {
            myJob = JobFactory.Create(defaultJob, necessities, false);
            // recordedLoc = myJob.currentLoc;
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
            Debug.LogWarning("Animatronic has no CharacterProfile assigned, cannot get CharacterName.");
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
            Debug.LogWarning("Animatronic has no current job assigned, cannot get JobName.");
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
        necessities.lastlocation = oldLocation;
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
        if (world?.aiMonitor != null)
            world.aiMonitor.OnJobSwitched -= HandleSwitchRequest;
    }
    
}