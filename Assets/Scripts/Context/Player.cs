using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public View CurrentView { get; private set; }
    [SerializeField] private View startingView;
    public bool IsDead { get; private set; } = false;

    public event Action<View> OnViewChanged;
    public event Action<Action, float> OnActionPerformed;

    private void Awake()
    {
        Instance = this;
        SetView(startingView);
    }

    public void SetView(View newView)
    {
        CurrentView = newView;
        OnViewChanged?.Invoke(newView);
        Debug.Log("Player is now looking at: " + newView);
    }

    public void Die()
    {
        if (IsDead) return;
        IsDead = true;
    }

    public void PerformAction(Action action, float actionTime)
    {
        OnActionPerformed?.Invoke(action, actionTime);
    }
}