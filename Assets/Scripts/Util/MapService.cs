using System.Collections.Generic;
using UnityEngine;

public class MapService
{
    public sealed class RuntimeMap
    {
        public class RNode { public LocName roomName; public List<REdge> edges = new(); }
        public class REdge { public RNode to; public float weight;}
        public Dictionary<LocName, RNode> nodes = new();
    }   

    public RuntimeMap BuildRuntimeMap(BaseMap MapBase, bool jitterWeights = true, DoorName lastFailedDoor = DoorName.None){

        var RunTimeMap = new RuntimeMap();

        foreach (var baseMapRoom in MapBase.Rooms)

            RunTimeMap.nodes[baseMapRoom.thisBaseMapRoomName] = new RuntimeMap.RNode { roomName = baseMapRoom.thisBaseMapRoomName };

        foreach (var baseMapRoom in MapBase.Rooms) {

            var currentRunTimeRoom = RunTimeMap.nodes[baseMapRoom.thisBaseMapRoomName];

            foreach (var baseMapEdge in baseMapRoom.edges) {

                var runTo = RunTimeMap.nodes[baseMapEdge.to.thisBaseMapRoomName];

                float randomValue = Random.Range(-0.5f, 0.5f);

                float newWeight = jitterWeights ? baseMapEdge.weight + (baseMapEdge.weight * randomValue) : baseMapEdge.weight;

                if(IFailedThatDoor(runTo, lastFailedDoor))
                {
                    newWeight *= 0.1f;
                }

                currentRunTimeRoom.edges.Add(new RuntimeMap.REdge { to = runTo, weight = newWeight,});
                
            }
        }
        return RunTimeMap;
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
        if (failedDoor == DoorName.None)
            return false;

        if ((failedDoor == DoorName.Left && (to.roomName == LocName.SecondOffice || to.roomName == LocName.WestHall_Far)) ||
            (failedDoor == DoorName.Right && (to.roomName == LocName.Kitchen || to.roomName == LocName.EastHall_Far)))
        {
            return true;
        }
        return false;
    }
}
