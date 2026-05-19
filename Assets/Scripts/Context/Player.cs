using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public View CurrentView { get; private set; }
    [SerializeField] private View startingView;
    public bool IsDead { get; private set; } = false;

    public event Action<View> OnViewChanged;
    public event Action<Action> OnActionPerformed;

    private void Awake()
    {
        Instance = this;
        SetView(startingView);
    }

    public void SetView(View newView)
    {
        CurrentView = newView;
        OnViewChanged?.Invoke(newView);
    }

    public void Die()
    {
        if (IsDead) return;
        IsDead = true;
    }

    public void PerformAction(Action action)
    {
        OnActionPerformed?.Invoke(action);
    }
}