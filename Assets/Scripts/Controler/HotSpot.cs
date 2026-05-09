using UnityEngine;

public class HotSpot : MonoBehaviour
{
    public World world;
    public View to;
    public Action action;

    public void EnvokeViewChange()
    {
        world.player.SetNewView(to);
    }

     public void EnvokeAction()
    {
        world.player.PerformAction(action);
    }
    
}
