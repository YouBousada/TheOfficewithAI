using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool IsDead { get; private set; } = false;

    [SerializeField] private GameObject inventoryIconPrefab;
    [SerializeField] private Transform inventoryContainer;
    [SerializeField] private Transform inspectionContainer;
    [SerializeField] private List<Item> inventory = new();
    [SerializeField] private int InventoryCapacity;
    [SerializeField] private CurrentView currentView;
    [SerializeField] private PlayerConfig config;
    [SerializeField] public ItemVariable ItemInHand;
    [SerializeField] private PickupSuccessEvent pickupSuccessEvent;
    [SerializeField] private ReturnItemEvent returnItemEvent;

    public bool InventoryFull => inventory.Count >= InventoryCapacity;
    public bool IsHoldingSomething => ItemInHand.item != null;
    public bool IsInspectingItem { get; private set; }

    private void Awake()
    {
        IsInspectingItem = false;

        if (inventoryIconPrefab == null)  Debug.LogWarning($"{name}: inventoryIconPrefab is not assigned.", this);
        if (inventoryContainer == null)   Debug.LogWarning($"{name}: inventoryContainer is not assigned.", this);
        if (inspectionContainer == null)  Debug.LogWarning($"{name}: inspectionContainer is not assigned.", this);

        SetView(config.startingview);
    }

    private void Update()
    {
        if (IsDead)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            if (IsHoldingSomething && !IsInspectingItem)
            {
                returnItemEvent.Raise(ItemInHand.item);
                ItemInHand.item = null;
            }
        }

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
                ItemInHand.item = null;
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
            inspectionDisplayImage.sprite = ItemInHand.item.InspectSprite;
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
        if (ItemInHand.item == null)
        {
            ItemInHand.item = item;
            inventory.Remove(item);
            BuildInventoryDisplay();
        }
    }

    public void ReturnHeldItemToInventory()
    {
        if (ItemInHand.item)
        {
            AddItemToInventory(ItemInHand.item);
            ItemInHand.item = null;
        }
    }
    public void AddItemToInventory(Item newItem)
    {
        if (!InventoryFull)
        {
            inventory.Add(newItem);
            BuildInventoryDisplay();
        }
    }

    public void RemoveItemFromInventory(Item item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            BuildInventoryDisplay();
        }
    }

    public void SetView(ViewName newView)
    {
        currentView.viewName = newView;
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

    public void HandleAttemptPickup(Item item)
    {
        if (InventoryFull || IsHoldingSomething) return;
        AddItemToInventory(item);
        pickupSuccessEvent.Raise(item);
    }

    public void Die(JumpscareEventData data)
    {
        if (IsDead) return;

        ClearInventoryDisplay();
        IsInspectingItem = false;
        GenerateInspectionGUI(IsInspectingItem);
        IsDead = true;
    }
}