using System;
using UnityEngine;
using UnityEngine.Events;

public class LocationChangedListener : AbstractGameEventListener<LocationChangedData>
{
    [Serializable] public class Response : UnityEvent<LocationChangedData> { }
    [SerializeField] Response response;

    public override void OnEventRaised(LocationChangedData data) => response.Invoke(data);
}
