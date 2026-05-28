using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(UnityEngine.UI.Button))]
public class InventoryItemButton : MonoBehaviour
{
    [SerializeField] private PlayerCommand command;
    [SerializeField] private Player player;
    [SerializeField] private Office office;
    [SerializeField] private PuzzleMaster puzzleMaster;
    private Button button;

    public void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(InvokeHoldItem);

        if (command == null)      Debug.LogWarning($"{name}: command is not assigned.", this);
        if (player == null)       Debug.LogWarning($"{name}: player is not assigned.", this);
        if (office == null)       Debug.LogWarning($"{name}: office is not assigned.", this);
        if (puzzleMaster == null) Debug.LogWarning($"{name}: puzzleMaster is not assigned.", this);
    }

    private void InvokeHoldItem()
    {
        command.Execute(player, office, puzzleMaster);
    }
}
