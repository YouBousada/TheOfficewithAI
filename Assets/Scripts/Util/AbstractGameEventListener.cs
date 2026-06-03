using UnityEngine;

public abstract class AbstractGameEventListener<T> : MonoBehaviour
{
    [SerializeField] AbstractGameEvent<T> gameEvent;

    void OnEnable() => gameEvent.Register(this);
    void OnDisable() => gameEvent.Unregister(this);

    public abstract void OnEventRaised(T val);
}
