using UnityEngine;
using UnityEngine.UI;

public class InventoryIcon : MonoBehaviour
{
    [SerializeField] private GameContext gameContext;
    private Image image;
    public void Awake()
    {
        gameContext.Player.OnReceiveItem += DisplayItemInInventory;
        gameContext.Player.OnLooseItem += ClearInventoryDisplay;
        image = GetComponent<Image>();
        image.enabled = false;
    }
    
    public void DisplayItemInInventory(Item item)
    {
        image.sprite = item.InventorySprite;
        image.enabled = true;
    }
    public void ClearInventoryDisplay(Item item)
    {
        image.enabled = false;
    }
    public void Oestroy()
    {
        gameContext.Player.OnReceiveItem -= DisplayItemInInventory;
        gameContext.Player.OnLooseItem -= DisplayItemInInventory;
    }
}
