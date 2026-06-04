using System;
using UnityEngine;
using UnityEngine.Events;

public class JobSwitchedListener : AbstractGameEventListener<JobSwitchedData>
{
    [Serializable] public class Response : UnityEvent<JobSwitchedData> { }
    [SerializeField] Response response;

    public override void OnEventRaised(JobSwitchedData data) => response.Invoke(data);
}
