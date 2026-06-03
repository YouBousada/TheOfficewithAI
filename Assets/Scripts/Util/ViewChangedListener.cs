using System;
using UnityEngine;
using UnityEngine.Events;

public class ViewChangedListener : AbstractGameEventListener<ViewName>
{
    [Serializable] public class Response : UnityEvent<ViewName> { }
    [SerializeField] Response response;

    public override void OnEventRaised(ViewName view) => response.Invoke(view);
}