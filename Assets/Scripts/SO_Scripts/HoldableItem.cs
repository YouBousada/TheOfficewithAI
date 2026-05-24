using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class HoldableItem : ScriptableObject
{
    [SerializeField] private ItemName itemName;
    [SerializeField] private Sprite inventorySprite;

    public ItemName ItemName => itemName;
    public Sprite InventorySprite => inventorySprite;

}
