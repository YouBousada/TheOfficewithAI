using System;
using UnityEngine;
using UnityEngine.Events;

public class UseItemListener : AbstractGameEventListener<Item>
{
    [Serializable] public class Response : UnityEvent<Item> { }
    [SerializeField] Response response;
    public override void OnEventRaised(Item data) => response.Invoke(data);
}
