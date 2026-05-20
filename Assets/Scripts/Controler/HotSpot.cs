using UnityEngine;

public class HotSpot : MonoBehaviour
{
    [SerializeField] private World world;
    [SerializeField] private View to;
    [SerializeField] private Action action;

    public void InvokeViewChange(){
        world.Player.SetView(to);
        Debug.Log("Player set view to " + to);
    }
    public void InvokeAction(){
        world.Player.PerformAction(action);
        Debug.Log("action taken: " + action);
    }

}
