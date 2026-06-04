using System.Collections.Generic;
using UnityEngine;

public class ActionHotspot : HotSpot
{
    [Header("ActionHotspot requirements")]
    [SerializeField] private PlayerCommand command;
    [SerializeField] private List<Item> AllowedItemsInHandToUse;
    // [SerializeField] private Office office;
    // [SerializeField] private PuzzleMaster puzzleMaster;

    protected override void Awake()
    {
        base.Awake();
        if (command == null)      Debug.LogWarning($"{name}: command is not assigned.", this);
        // if (office == null)       Debug.LogWarning($"{name}: office is not assigned.", this);
        // if (puzzleMaster == null) Debug.LogWarning($"{name}: puzzleMaster is not assigned.", this);
        button.onClick.AddListener(InvokeAction);
    }

    private void InvokeAction()
    {
        if (command == null)
        {
            Debug.LogWarning($"{name} cannot perform an action because no command is assigned.", this);
            return;
        }

        // if (player.IsInspectingItem)
        //     return;

        if (AllowedItemsInHandToUse.Count > 0)
        {
            // if (AllowedItemsInHandToUse.Contains(player.itemInHand))
            //     command.Execute(player, office, puzzleMaster);
        }
        else
        {
            // command.Execute(player, office, puzzleMaster);
        }
    }
}
