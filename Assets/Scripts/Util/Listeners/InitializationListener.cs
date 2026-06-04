using System;
using UnityEngine;
using UnityEngine.Events;

public class InitializationListener : AbstractGameEventListener<InitializationData>
{
    [Serializable] public class Response : UnityEvent<InitializationData> { }
    [SerializeField] Response response;

    public override void OnEventRaised(InitializationData data) => response.Invoke(data);
}
