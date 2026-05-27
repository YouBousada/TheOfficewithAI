using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(UnityEngine.UI.Button))]
public class InventoryItemButton : MonoBehaviour
{
    [SerializeField] private GameContext cntx;
    private Button button;
    [SerializeField] private PlayerCommand command;
    public void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(InvokeHoldItem);
    }
    private void InvokeHoldItem()
    {
        command.Execute(cntx);
    }
}
