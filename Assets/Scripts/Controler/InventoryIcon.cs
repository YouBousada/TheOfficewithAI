using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]

public class InventoryIcon : MonoBehaviour
{
    private Image image;
    private Button button;
    private Action onHold;

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

    public void Display(Item item, Action onHold)
    {
        this.onHold = onHold;
        image.sprite = item.InventorySprite;
        image.enabled = true;
    }

    public void TellPlayerToHoldMe()
    {
        onHold?.Invoke();
    }
}
