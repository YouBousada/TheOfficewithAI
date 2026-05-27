using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ActionHotspot : HotSpot
{
    [Header("ActionHotspot requirements")]
    [SerializeField] private PlayerCommand command;
    [SerializeField] private List<Item> AllowedItemsInHandToUse;

    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(InvokeAction);
    }
    private void InvokeAction()
    {
        if (command == null)
        {
            Debug.LogWarning($"{name} cannot perform an action because no command is assigned.", this);
            return;
        }

        if (Player.Instance.IsInspectingItem)
        {
            return;
        }

        if(AllowedItemsInHandToUse.Count > 0)
        {
            if (AllowedItemsInHandToUse.Contains(Player.Instance.itemInHand))
            {
                command.Execute(gameContext);
            }
        }
        else
        {
            command.Execute(gameContext);
        }
    }
}
