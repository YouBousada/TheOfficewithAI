using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public View CurrentView { get; private set; }
    [SerializeField] private View startingView;
    public bool IsDead { get; private set; } = false;

    public event Action<View> OnViewChanged;
    public event Action<ActionType> OnActionPerformed;

    private HoldableItem ItemInHand;

    private void Awake()
    {
        Instance = this;
        SetView(startingView);
    }

    private HoldableItem GetItemInHand()
    {
        return ItemInHand;
    }

    private bool HoldItem(HoldableItem newItem)
    {
        if(ItemInHand == null)
        {
            ItemInHand = newItem;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ClearItem()
    {
        ItemInHand = null;
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

    public void PerformAction(ActionType action)
    {
        OnActionPerformed?.Invoke(action);
    }
}