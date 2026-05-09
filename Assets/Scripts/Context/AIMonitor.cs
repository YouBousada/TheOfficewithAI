using UnityEngine;
using System;

public class AIMonitor : MonoBehaviour
{
    public void HandleLocationChange(CharacterName who, LocName from, LocName to)
    {
        Debug.Log($"[{Time.time:F2}] {who} moved from {from} to {to}");
        OnLocationChanged?.Invoke(who, from, to);
    }

    public void HandleBehaviourChange(CharacterName who, Behaviour oldBehaviour, Behaviour newBehaviour)
    {
        Debug.Log($"[{Time.time:F2}] {who} changed behaviour from {oldBehaviour}ing to {newBehaviour}ing");
        OnBehaviourChanged?.Invoke(who, oldBehaviour, newBehaviour);
    }

    public void HandleJobSwitch(CharacterName who, JobName fromJob, JobName toJob)
    {
        Debug.Log($"<color=cyan>[INIT]</color> [{Time.time:F2}] {who} switched job from {fromJob} to {toJob}");
        OnJobSwitched?.Invoke(who, fromJob, toJob);
    }

    public void HandleInitialization(CharacterName who, LocName location, Behaviour behaviour, JobName job)
    {
        // Debug.Log($"<color=yellow>[JOB]</color>[{Time.time:F2}] {who} initialized at {location} with {behaviour} behaviour in job {job}");
        OnInitialization?.Invoke(who, location, behaviour, job);
    }

    public void HandleDeath(CharacterName who, LocName location)
    {
        Debug.Log($"<color=red><b>[DEATH]</b></color>[{Time.time:F2}] {who} has killed the player from {location}.");
        OnDeath?.Invoke(who, location);
    }

    public event Action<CharacterName, LocName, LocName> OnLocationChanged;
    public event Action<CharacterName, Behaviour, Behaviour> OnBehaviourChanged;
    public event Action<CharacterName, JobName, JobName> OnJobSwitched;
    public event Action<CharacterName, LocName, Behaviour, JobName> OnInitialization;
    public event Action<CharacterName, LocName> OnDeath;

}