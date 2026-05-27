using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public ViewName CurrentView { get; private set; }
    public bool IsDead { get; private set; } = false;

    [SerializeField] private ViewName startingView;
    [SerializeField] private GameObject inventoryIconPrefab;
    [SerializeField] private Transform inventoryContainer;

    public event Action<ViewName> OnViewChanged;
    public event Action<ActionType> OnActionPerformed;
    public event Action<Item> OnReceiveItem;
    public event Action<Item> OnRemoveItemFromInventory;

    private List<Item> inventory = new();
    public bool InventoryFull => inventory.Count >= 3;
    public bool IsHoldingSomething => itemInHand != null;
    public Item itemInHand { get; private set; }

    private void Awake()
    {
        Instance = this;
        SetView(startingView);
    }

    public void HoldItem(Item item)
    {
        if (itemInHand == null)
        {
            itemInHand = item;
            inventory.Remove(item);
            BuildInventoryDisplay();
        }
    }
    public void AddItemToInventory(Item newItem)
    {
        if (!InventoryFull && itemInHand == null)
        {
            inventory.Add(newItem);
            OnReceiveItem?.Invoke(newItem);
            BuildInventoryDisplay();
        }
    }

    public void RemoveItemFromInventory(Item item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            OnRemoveItemFromInventory?.Invoke(item);
            BuildInventoryDisplay();
        }
    }

    public void SetView(ViewName newView)
    {
        CurrentView = newView;
        OnViewChanged?.Invoke(newView);
        Debug.Log("Player is now looking at: " + newView);
    }

    private void BuildInventoryDisplay()
    {
        foreach (Transform child in inventoryContainer)
            Destroy(child.gameObject);

        foreach (Item item in inventory)
        {
            var icon = Instantiate(inventoryIconPrefab, inventoryContainer).GetComponent<InventoryIcon>();
            icon.Display(item);
        }
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