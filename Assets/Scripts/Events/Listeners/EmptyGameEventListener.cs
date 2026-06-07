using UnityEngine;
using UnityEngine.Events;

public class EmptyGameEventListener : MonoBehaviour
{
    [SerializeField] private EmptyGameEvent gameEvent;
    [SerializeField] private UnityEvent response;

    void OnEnable() => gameEvent.Register(this);
    void OnDisable() => gameEvent.Unregister(this);

    public void OnEventRaised() => response.Invoke();
}
