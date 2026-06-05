using System;
using UnityEngine;

public struct JobNecessities
{
    public AiEvents aiEvents;
    public CurrentView currentPlayerView;
    public Office officeStates;
    public ActiveCameraVariable activeCamera;
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
    private AnimatronicRegistry animatronicRegistry;
    [SerializeField] private AiEvents aiEvents;
    [SerializeField] private CurrentView currentPlayerView;
    [SerializeField] private Office officeStates;
    [SerializeField] private ActiveCameraVariable activeCamera;
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
        if (aiEvents == null)      Debug.LogWarning($"{name}: aiEvents is not assigned.", this);
        if (officeStates == null)  Debug.LogWarning($"{name}: officeStates is not assigned.", this);
        if (activeCamera == null)  Debug.LogWarning($"{name}: activeCamera is not assigned.", this);
        if (mapGraphs == null)     Debug.LogWarning($"{name}: mapGraphs is not assigned.", this);
        if (profile == null)       Debug.LogWarning($"{name}: profile is not assigned.", this);

        animatronicRegistry = AnimatronicRegistry.Instance;

        if (profile != null) recordedLoc = profile.NightStartingRoom;
        if (profile != null)
            animatronicRegistry.RegisterAnimatronic(profile.CharacterName, this);
    }

    void Start()
    {
        memories = new AnimatronicMemory { lastFailedDoor = DoorName.NONE };

        necessities = new JobNecessities
        {
            aiEvents            = aiEvents,
            currentPlayerView   = currentPlayerView,
            officeStates        = officeStates,
            activeCamera        = activeCamera,
            animatronicRegistry = animatronicRegistry,
            mapGraphs           = mapGraphs,
            givenProfile        = profile,
            LastLocation        = recordedLoc,
            memories            = memories,
            aiLevel             = Ailevel
        };

        myJob ??= JobFactory.Create(GetDefaultJobForThisChar(), necessities, false);
    }

    void Update()
    {
        myJob?.PerformJob();
    }

    void LateUpdate()
    {
        if (!hasPendingSwitch) return;
        hasPendingSwitch = false;

        necessities.LastLocation = recordedLoc;
        myJob = JobFactory.Create(pendingJob, necessities, true);
        recordedLoc = myJob.currentLoc;
    }

    void OnDestroy()
    {
        if (profile != null)
            animatronicRegistry.UnregisterAnimatronic(profile.CharacterName, this);
    }

    private JobName GetDefaultJobForThisChar()
    {
        return profile.JobsByPreference[0];
    }

    public void HandleSwitchRequest(JobSwitchedData data)
    {
        CharacterName who = data.who;
        JobName toJob     = data.toJob;

        if (who != profile.CharacterName) return;
        if (hasPendingSwitch) return;
        pendingJob = toJob;
        hasPendingSwitch = true;
    }

    public void HandleLocationChange(LocationChangedData data)
    {
        CharacterName who    = data.who;
        RoomName newLocation = data.to;

        if (who != profile.CharacterName) return;
        if (myJob != null)
            recordedLoc = newLocation;
    }

    public void HandleDeath(JumpscareEventData data)
    {
        myJob = null;
    }

    public RoomName GetCurrentLocation()
    {
        return recordedLoc;
    }

    public CharacterName GetCharacterName()
    {
        if (profile != null)
        {
            return profile.CharacterName;
        }
        else
        {
            return CharacterName.NONE;
        }
    }

    public BehaviourName GetCurrentBehaviour()
    {
        if (myJob != null)
        {
            return myJob.currentBehaviour;
        }
        else
        {
            return BehaviourName.NONE;
        }
    }

    public JobName GetCurrentJobName()
    {
        if (myJob != null)
        {
            return myJob.JobName;
        }
        else
        {
            return JobName.NONE;
        }
    }
}
