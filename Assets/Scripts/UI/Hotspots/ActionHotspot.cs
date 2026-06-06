using System.Collections.Generic;
using UnityEngine;

public class ActionHotspot : HotSpot
{
    [Header("ActionHotspot requirements")]
    [SerializeField] private ItemVariable itemPlayerIsHolding;
    [SerializeField] private List<Item> AllowedItemsInHandToUse;
    [SerializeField] private PlayerCommand command;
    [SerializeField] private ActionType actionType;
    [SerializeField] private HotSpotCoolDownEvent hotSpotCoolDownEvent;

    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(InvokeAction);
    }

    private void InvokeAction()
    {
        if (AllowedItemsInHandToUse.Count > 0)
        {
            if (!AllowedItemsInHandToUse.Contains(itemPlayerIsHolding.item)) return;
        }

        command.Execute();
        hotSpotCoolDownEvent.Raise(actionType.HotspotDisableDuration);
    }
}
