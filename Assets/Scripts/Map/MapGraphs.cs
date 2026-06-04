using UnityEngine;

[CreateAssetMenu(fileName = "MapGraphs", menuName = "Scriptable Objects/MapGraphs")]
public class MapGraphs : ScriptableObject
{
    [field: SerializeField] public BaseMap RunningBaseMap { get; private set; }
    [field: SerializeField] public BaseMap EncroachBaseMap { get; private set; }
    [field: SerializeField] public BaseMap LeaveBaseMap { get; private set; }
}
