using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class ActionHotspot : HotSpot
{
    [Header("ActionHotspot requirements")]
    [SerializeField] private PlayerCommand command;

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

        command.Execute(gameContext);
    }
}
