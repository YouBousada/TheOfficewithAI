using System;
using UnityEngine;

public struct JobNecessities
{
    public AiMonitor aiMonitor;
    public Office officeStates;
    public Player player;
    public CameraSystem cameraSystem;
    public AnimatronicRegistry animatronicRegistry;
    public MapGraphs mapGraphs;
    public CharacterProfile givenProfile;
    public RoomName LastLocation;
    public AnimatronicMemory memories;
    public int aiLevel;
}

public class AnimatronicMemory
{
    public DoorName lastFailedDoor;
}

public class Animatronic : MonoBehaviour
{
    [Header("Animatronic requirements")]
    [SerializeField] private AnimatronicRegistry animatronicRegistry;
    [SerializeField] private AiMonitor aiMonitor;
    [SerializeField] private Office officeStates;
    [SerializeField] private Player player;
    [SerializeField] private CameraSystem cameraSystem;
    [SerializeField] private MapGraphs mapGraphs;
    [SerializeField] private CharacterProfile profile;
    [SerializeField] private int Ailevel;

    private BaseJob myJob;
    private RoomName recordedLoc;
    private JobNecessities necessities;
    private AnimatronicMemory memories;
    private bool hasPendingSwitch;
    private JobName pendingJob;

    void Awake()
    {
        if (animatronicRegistry == null) Debug.LogWarning($"{name}: animatronicRegistry is not assigned.", this);
        if (aiMonitor == null)    Debug.LogWarning($"{name}: aiMonitor is not assigned.", this);
        if (officeStates == null) Debug.LogWarning($"{name}: officeStates is not assigned.", this);
        if (player == null)       Debug.LogWarning($"{name}: player is not assigned.", this);
        if (cameraSystem == null) Debug.LogWarning($"{name}: cameraSystem is not assigned.", this);
        if (mapGraphs == null)    Debug.LogWarning($"{name}: mapGraphs is not assigned.", this);
        if (profile == null)      Debug.LogWarning($"{name}: profile is not assigned.", this);

        if (profile != null) recordedLoc = profile.nightStartingRoom;
        if (animatronicRegistry != null && profile != null)
            animatronicRegistry.RegisterAnimatronic(profile.characterName, this);
    }

    void OnEnable()
    {
        if (aiMonitor == null) return;
        aiMonitor.OnJobSwitched += HandleSwitchRequest;
        aiMonitor.OnLocationChanged += HandleLocationChange;
        aiMonitor.OnDeath += HandleDeath;
    }

    void OnDisable()
    {
        if (aiMonitor == null) return;
        aiMonitor.OnJobSwitched -= HandleSwitchRequest;
        aiMonitor.OnLocationChanged -= HandleLocationChange;
        aiMonitor.OnDeath -= HandleDeath;
    }

    void Start()
    {
        var defaultJob = GetDefaultJobForThisChar();

        memories = new AnimatronicMemory { lastFailedDoor = DoorName.NONE };

        necessities = new JobNecessities
        {
            aiMonitor          = aiMonitor,
            officeStates       = officeStates,
            player             = player,
            cameraSystem       = cameraSystem,
            animatronicRegistry = animatronicRegistry,
            mapGraphs          = mapGraphs,
            givenProfile       = profile,
            LastLocation       = recordedLoc,
            memories           = memories,
            aiLevel            = Ailevel
        };

        if (myJob == null)
            myJob = JobFactory.Create(defaultJob, necessities, false);
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

    public RoomName GetCurrentLocation()
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

    void HandleLocationChange(CharacterName who, RoomName oldLocation, RoomName newLocation, BehaviourName behaviour)
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

        RoomName oldLocation = recordedLoc;
        necessities.LastLocation = oldLocation;
        myJob = JobFactory.Create(pendingJob, necessities, true);
        recordedLoc = myJob.currentLoc;
    }

    void HandleDeath(CharacterName who, RoomName location)
    {
        if (who == profile.characterName){return;}
        
        if (myJob != null)
        {
            myJob = null;
        }
        
    }
    void OnDestroy()
    {
        if (aiMonitor != null)
            aiMonitor.OnJobSwitched -= HandleSwitchRequest;

        if (animatronicRegistry != null && profile != null)
            animatronicRegistry.UnregisterAnimatronic(profile.characterName, this);
    }
    
}
