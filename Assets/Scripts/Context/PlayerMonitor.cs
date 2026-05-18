using System;
using UnityEngine;

public class PlayerMonitor : MonoBehaviour
{
    public Player player;

    public void Start()
    {
        OnViewChanged?.Invoke(player.startingView);
    }

    public void HandleNewView(View newView)
    {
        OnViewChanged?.Invoke(newView);
        Debug.Log("View changed to: " + newView);
    }

    public void HandleAction(Action action)
    {
        OnActionPerformed?.Invoke(action);
        Debug.Log("Action performed: " + action);
    }
    public event Action <View> OnViewChanged;
    public event Action <Action> OnActionPerformed;
}
