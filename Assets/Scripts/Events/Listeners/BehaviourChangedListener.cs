using System;
using UnityEngine;
using UnityEngine.Events;

public class BehaviourChangedListener : AbstractGameEventListener<BehaviourChangedData>
{
    [Serializable] public class Response : UnityEvent<BehaviourChangedData> { }
    [SerializeField] Response response;

    public override void OnEventRaised(BehaviourChangedData data) => response.Invoke(data);
}
