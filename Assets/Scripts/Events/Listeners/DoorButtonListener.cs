using System;
using UnityEngine;
using UnityEngine.Events;

public class DoorButtonListener : AbstractGameEventListener<DoorButtonName>
{
    [Serializable] public class Response : UnityEvent<DoorButtonName> { }
    [SerializeField] Response response;
    public override void OnEventRaised(DoorButtonName data) => response.Invoke(data);
}
