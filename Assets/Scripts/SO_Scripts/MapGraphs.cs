using UnityEngine;

[CreateAssetMenu(fileName = "MapGraphs", menuName = "Scriptable Objects/MapGraphs")]
public class MapGraphs : ScriptableObject
{
    [field: SerializeField] public BaseMap runningBaseMap { get; private set; }
    [field: SerializeField] public BaseMap encroachBaseMap { get; private set; }
    [field: SerializeField] public BaseMap leaveBaseMap { get; private set; }
}
