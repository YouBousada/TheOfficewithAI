using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public View currentView;
    public View startingView;
    public bool isDead = false;

    public event Action<View> OnViewChanged;
    public event Action<Action> OnActionPerformed;

    private void Start()
    {
        SetView(startingView);
    }

    public void SetView(View newView)
    {
        currentView = newView;
        OnViewChanged?.Invoke(newView);
    }

    public void PerformAction(Action action)
    {
        OnActionPerformed?.Invoke(action);
    }
}