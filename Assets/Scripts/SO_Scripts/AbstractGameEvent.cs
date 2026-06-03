using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractGameEvent<T> : ScriptableObject
{
    readonly List<AbstractGameEventListener<T>> listeners = new();
    public void Raise(T val)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(val);
    }
    public void Register(AbstractGameEventListener<T> l)   => listeners.Add(l);
    public void Unregister(AbstractGameEventListener<T> l) => listeners.Remove(l);
}
