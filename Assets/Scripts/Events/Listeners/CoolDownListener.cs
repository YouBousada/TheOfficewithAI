using System;
using UnityEngine;
using UnityEngine.Events;

public class CoolDownListener : AbstractGameEventListener<float>
{
    [Serializable] public class Response : UnityEvent<float> { }
    [SerializeField] Response response;

    public override void OnEventRaised(float data) => response.Invoke(data);
}
