using UnityEngine;

public class HotSpot : MonoBehaviour
{
    public World world;
    public View to;
    public Action action;

    public void EnvokeViewChange(){
        world.player.SetView(to);
        Debug.Log("Player set view to " + to);
    }
    public void EnvokeAction(){
        world.player.PerformAction(action);
        Debug.Log("action taken: " + action);
    }

}
