using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    public ViewName CurrentView { get; private set; }
    public bool IsDead { get; private set; } = false;

    [SerializeField] private ViewName startingView;
    [SerializeField] private GameObject inventoryIconPrefab;
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private Transform inspectionContainer;
    [SerializeField] private List<Item> inventory = new();
    [SerializeField] private int InventoryCapacity;

    public event Action<ViewName> OnViewChanged;
    public event Action<ActionType> OnActionPerformed;
    public event Action<Item> OnReceiveItem;
    public event Action<Item> OnRemoveItemFromInventory;
    public event Action<Item> OnReturnItem;

    public bool InventoryFull => inventory.Count >= InventoryCapacity;
    public bool IsHoldingSomething => itemInHand != null;
    public Item itemInHand { get; private set; }
    public bool IsInspectingItem { get; private set; }

    private void Awake()
    {
        Instance = this;
        IsInspectingItem = false;
        SetView(startingView);
    }

    public void Update()
    {
        if (IsDead)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            if (!IsInspectingItem)
            {
                ReturnHeldItemToInventory();
            }
        }

        // if (Input.GetMouseButtonDown(0))
        // {
        //     if(IsHoldingSomething && !IsInspectingItem)
        //     {
        //         itemInHand.Use.Execute(GameContext.Instance);
        //     }
        // }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsHoldingSomething)
            {
                UpdateInspectionState();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (IsHoldingSomething && !IsInspectingItem)
            {
                OnReturnItem?.Invoke(itemInHand);
                itemInHand = null;
            }
        }
    }

    private void UpdateInspectionState()
    {
        IsInspectingItem = !IsInspectingItem;
        GenerateInspectionGUI(IsInspectingItem);
    }

    public void GenerateInspectionGUI(bool inspecting)
    {
        if (inspecting)
        {
            var go = new GameObject("BlackOverlay");
            go.transform.SetParent(inspectionContainer, false);

            var image = go.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0.9f);

            var rect = image.rectTransform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var inspectionDisplayObject = new GameObject("InspectionDisplay");
            inspectionDisplayObject.transform.SetParent(inspectionContainer, false);

            var inspectionDisplayImage = inspectionDisplayObject.AddComponent<Image>();
            inspectionDisplayImage.sprite = itemInHand.InspectSprite;
            inspectionDisplayImage.preserveAspect = true;

            var inspectionRect = inspectionDisplayImage.rectTransform;
            inspectionRect.anchorMin = Vector2.zero;
            inspectionRect.anchorMax = Vector2.one;
            inspectionRect.offsetMin = new Vector2(30, 30);
            inspectionRect.offsetMax = new Vector2(-30, -30);
        }
        else
        {
            foreach (Transform child in inspectionContainer)
                Destroy(child.gameObject);
        }

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
    
    private void UseItem()
    {
        
    }

    public void ReturnHeldItemToInventory()
    {
        if (itemInHand)
        {
            AddItemToInventory(itemInHand);
            itemInHand = null;
        }
    }
    public void AddItemToInventory(Item newItem)
    {
        if (!InventoryFull)
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
        ClearInventoryDisplay();

        foreach (Item item in inventory)
        {
            var icon = Instantiate(inventoryIconPrefab, inventoryContainer).GetComponent<InventoryIcon>();
            icon.Display(item, () => HoldItem(item));
        }
    }

    private void ClearInventoryDisplay()
    {
        foreach (Transform child in inventoryContainer)
            Destroy(child.gameObject);
    }

    public void Die()
    {
        if (IsDead) return;

        ClearInventoryDisplay();
        IsInspectingItem = false;
        GenerateInspectionGUI(IsInspectingItem);
        IsDead = true;
    }

    public void PerformAction(ActionType action)
    {
        OnActionPerformed?.Invoke(action);
    }
}