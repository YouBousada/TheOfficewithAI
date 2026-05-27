using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class HeldItemImage : MonoBehaviour
{
    private Image heldItemImage;
    private Canvas canvas;

    void Awake()
    {
        heldItemImage = GetComponent<Image>();
        heldItemImage.preserveAspect = true;
        canvas = GetComponentInParent<Canvas>();
    }

    void Update() {

        if (Player.Instance.IsHoldingSomething && !Player.Instance.IsInspectingItem && !Player.Instance.IsDead)
        {
            heldItemImage.enabled = true;
            heldItemImage.sprite = Player.Instance.itemInHand.InventorySprite;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint);
            heldItemImage.rectTransform.localPosition = localPoint;
        }
        else
        {
            heldItemImage.enabled = false;
        }
    }

}
