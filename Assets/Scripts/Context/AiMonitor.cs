using UnityEngine;
using System;

public class AiMonitor : MonoBehaviour
{
    public static AiMonitor Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void HandleLocationChange(CharacterName who, LocName from, LocName to, BehaviourName currentBehaviour)
    {
        Debug.Log($"[{Time.time:F2}] {who} moved from {from} to {to}");
        OnLocationChanged?.Invoke(who, from, to, currentBehaviour);
    }

    public void HandleBehaviourChange(CharacterName who, BehaviourName oldBehaviour, BehaviourName newBehaviour, LocName currentLocation)
    {
        // Debug.Log($"[{Time.time:F2}] {who} changed behaviour from {oldBehaviour}ing to {newBehaviour}ing");
        OnBehaviourChanged?.Invoke(who, oldBehaviour, newBehaviour, currentLocation);
    }

    public void HandleJobSwitch(CharacterName who, JobName fromJob, JobName toJob)
    {
        // Debug.Log($"<color=cyan>[INIT]</color> [{Time.time:F2}] {who} switched job from {fromJob} to {toJob}");
        OnJobSwitched?.Invoke(who, fromJob, toJob);
    }

    public void HandleInitialization(CharacterName who, LocName location, BehaviourName behaviour, JobName job)
    {
        // Debug.Log($"<color=yellow>[JOB]</color>[{Time.time:F2}] {who} initialized at {location} with {behaviour} behaviour in job {job}");
        OnInitialization?.Invoke(who, location, behaviour, job);
    }

    public void HandleDeath(CharacterName who, LocName location)
    {
        // Debug.Log($"<color=red><b>[DEATH]</b></color>[{Time.time:F2}] {who} has killed the player from {location}.");
        OnDeath?.Invoke(who, location);
    }

    public event Action<CharacterName, LocName, LocName, BehaviourName> OnLocationChanged;
    public event Action<CharacterName, BehaviourName, BehaviourName, LocName> OnBehaviourChanged;
    public event Action<CharacterName, JobName, JobName> OnJobSwitched;
    public event Action<CharacterName, LocName, BehaviourName, JobName> OnInitialization;
    public event Action<CharacterName, LocName> OnDeath;

}
