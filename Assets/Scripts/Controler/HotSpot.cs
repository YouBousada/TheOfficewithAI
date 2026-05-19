using UnityEngine;

public class HotSpot : MonoBehaviour
{
    [SerializeField] private World world;
    [SerializeField] private View to;
    [SerializeField] private Action action;

    public void EnvokeViewChange(){
        world.Player.SetView(to);
        Debug.Log("Player set view to " + to);
    }
    public void EnvokeAction(){
        world.Player.PerformAction(action);
        Debug.Log("action taken: " + action);
    }

}
