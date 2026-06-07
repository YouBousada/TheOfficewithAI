using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EmptyGameEvent", menuName = "Events/EmptyGameEvent")]
public class EmptyGameEvent : ScriptableObject
{
    private readonly List<EmptyGameEventListener> listeners = new();

    public void Raise()
    {
        foreach (var l in listeners) l.OnEventRaised();
    }

    public void Register(EmptyGameEventListener l) => listeners.Add(l);
    public void Unregister(EmptyGameEventListener l) => listeners.Remove(l);
}
