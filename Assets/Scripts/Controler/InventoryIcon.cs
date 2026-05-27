using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]

public class InventoryIcon : MonoBehaviour
{
    private Image image;
    private Button button;
    private Item item;
    public void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();

        image.enabled = false;
    }

    public void Start()
    {
        button.onClick.AddListener(TellPlayerToHoldMe);
    }

    public void Display(Item item)
    {
        this.item = item;
        image.sprite = this.item.InventorySprite;
        image.enabled = true;
    }

    public void TellPlayerToHoldMe()
    {
        Player.Instance.HoldItem(item);
    }
}
