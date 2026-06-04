using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItem", menuName = "Scriptable Objects/InventoryItem")]
public class Item : ScriptableObject
{
    [SerializeField] private ItemName itemName;
    [SerializeField] private Sprite inventorySprite;
    [SerializeField] private Sprite inspectSprite;

    public ItemName ItemName => itemName;
    public Sprite InventorySprite => inventorySprite;
    public Sprite InspectSprite => inspectSprite;

}
