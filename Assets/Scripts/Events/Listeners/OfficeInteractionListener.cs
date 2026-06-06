using System;
using UnityEngine;
using UnityEngine.Events;

public class EvironmentInteractionListener : AbstractGameEventListener<EvironmentInteractionName>
{
    [Serializable] public class Response : UnityEvent<EvironmentInteractionName> { }
    [SerializeField] Response response;

    public override void OnEventRaised(EvironmentInteractionName data) => response.Invoke(data);
}
