using System;
using UnityEngine;
using UnityEngine.Events;

public class CameraChangedListener : AbstractGameEventListener<RoomName>
{
    [Serializable] public class Response : UnityEvent<RoomName> { }
    [SerializeField] Response response;

    public override void OnEventRaised(RoomName data) => response.Invoke(data);
}
