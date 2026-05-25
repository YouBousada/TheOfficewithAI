using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public ViewName CurrentView { get; private set; }
    [SerializeField] private ViewName startingView;
    public bool IsDead { get; private set; } = false;

    public event Action<ViewName> OnViewChanged;
    public event Action<ActionType> OnActionPerformed;
    public event Action<Item> OnReceiveItem;
    public event Action<Item> OnLooseItem;

    private Item ItemInHand;

    private void Awake()
    {
        Instance = this;
        SetView(startingView);
    }

    public Item GetItem()
    {
        return ItemInHand;
    }

    public bool Give(Item newItem)
    {
        if(ItemInHand == null)
        {
            ItemInHand = newItem;
            OnReceiveItem?.Invoke(newItem);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ClearItem()
    {
        OnReceiveItem?.Invoke(ItemInHand);
        ItemInHand = null;
    }

    public void SetView(ViewName newView)
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