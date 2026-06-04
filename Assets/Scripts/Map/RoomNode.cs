using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNode", menuName = "Scriptable Objects/RoomNode")]
public class RoomNode : ScriptableObject
{

    public RoomName thisBaseMapRoomName;
    public List<Edge> edges = new();
    
}
