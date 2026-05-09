using System;
using UnityEngine;

public class PlayerMonitor : MonoBehaviour
{
    public Player player;

    public void Start()
    {
        OnViewChanged?.Invoke(player.startingView, 0f);
    }

    public void HandleNewView(View newView, float time)
    {
        OnViewChanged?.Invoke(newView, time);
        Debug.Log("View changed to: " + newView);
    }

    public void HandleAction(Action action)
    {
        OnActionPerformed?.Invoke(action);
        Debug.Log("Action performed: " + action);
    }
    public event Action <View, float> OnViewChanged;
    public event Action <Action> OnActionPerformed;
}
