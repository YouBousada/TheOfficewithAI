using UnityEngine;

[CreateAssetMenu(fileName = "Edge", menuName = "Scriptable Objects/Edge")]
public class Edge : ScriptableObject
{
    public RoomNode to;

    public int weight; 
}
