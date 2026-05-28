using System.Collections.Generic;
using UnityEngine;

public class MapService
{
    private const float WeightJitterRange = 0.5f;
    private const float FailedDoorWeightMultiplier = 0.1f;

    public sealed class RuntimeMap
    {
        public class RNode { public LocName roomName; public List<REdge> edges = new(); }
        public class REdge { public RNode to; public float weight; }
        public Dictionary<LocName, RNode> nodes = new();
    }

    public RuntimeMap BuildRuntimeMap(BaseMap MapBase, bool jitterWeights = true, DoorName lastFailedDoor = DoorName.NONE)
    {
        var runTimeMap = new RuntimeMap();

        foreach (var baseMapRoom in MapBase.Rooms)
            runTimeMap.nodes[baseMapRoom.thisBaseMapRoomName] = new RuntimeMap.RNode { roomName = baseMapRoom.thisBaseMapRoomName };

        foreach (var baseMapRoom in MapBase.Rooms)
        {
            var currentRunTimeRoom = runTimeMap.nodes[baseMapRoom.thisBaseMapRoomName];

            foreach (var baseMapEdge in baseMapRoom.edges)
            {
                var runTo = runTimeMap.nodes[baseMapEdge.to.thisBaseMapRoomName];

                float randomValue = Random.Range(-WeightJitterRange, WeightJitterRange);
                float newWeight = jitterWeights ? baseMapEdge.weight + (baseMapEdge.weight * randomValue) : baseMapEdge.weight;

                if (IFailedThatDoor(runTo, lastFailedDoor))
                    newWeight *= FailedDoorWeightMultiplier;

                currentRunTimeRoom.edges.Add(new RuntimeMap.REdge { to = runTo, weight = newWeight });
            }
        }
        return runTimeMap;
    }

    public RuntimeMap.RNode ChooseNext(RuntimeMap.RNode node)
    {
        if (node == null || node.edges == null || node.edges.Count == 0)
            return node;

        float sum = 0f;
        foreach (var e in node.edges) sum += Mathf.Max(0f, e.weight);
        if (sum <= 0f) return node;

        double roll = Random.Range(0f, sum);

        foreach (var edge in node.edges)
        {
            if (roll < edge.weight) return edge.to;
            roll -= edge.weight;
        }

        return node.edges[^1].to;
    }

    private bool IFailedThatDoor(RuntimeMap.RNode to, DoorName failedDoor)
    {
        if (failedDoor == DoorName.NONE) return false;

        return (failedDoor == DoorName.LEFT && (to.roomName == LocName.MANAGEMENT || to.roomName == LocName.WEST_HALL_FAR)) ||
               (failedDoor == DoorName.RIGHT && (to.roomName == LocName.KITCHEN || to.roomName == LocName.EAST_HALL_FAR));
    }
}
