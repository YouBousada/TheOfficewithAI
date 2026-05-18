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

        if (action == Action.OpenLeftDoor) world.SetLeftDoor(true);
        else if (action == Action.CloseLeftDoor) world.SetLeftDoor(false);
        else if (action == Action.OpenRightDoor) world.SetRightDoor(true);
        else if (action == Action.CloseRightDoor) world.SetRightDoor(false);
    }
    
}
