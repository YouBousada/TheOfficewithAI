using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseMap", menuName = "Scriptable Objects/BaseMap")]
public class BaseMap : ScriptableObject
{
    public List<RoomNode> Rooms = new();
}
