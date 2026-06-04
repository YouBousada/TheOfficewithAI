using System;
using UnityEngine;
using UnityEngine.Events;

public class JumpscareListener : AbstractGameEventListener<JumpscareEventData>
{
    [Serializable] public class Response : UnityEvent<JumpscareEventData> { }
    [SerializeField] Response response;

    public override void OnEventRaised(JumpscareEventData jumpscareEventData) => response.Invoke(jumpscareEventData);
}