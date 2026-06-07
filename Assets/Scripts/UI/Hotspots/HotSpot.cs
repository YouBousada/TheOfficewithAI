using System.Collections;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public abstract class HotSpot : OverLayObject
{
    [Header("Hotspot requirements")]
    protected Button button;
    protected CanvasGroup canvasGroup;
    private Coroutine actionCooldownCoroutine;
    protected bool isLocked {get; private set;}

    public bool CanUseHotspot => ShouldBeVisible && !isLocked;

    protected override void Awake()
    {
        base.Awake();

        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    //keep these 2 here
    public void HandleCoolDown(float coolDownTime)
    {
        if (button == null) return;

        if (actionCooldownCoroutine != null)
            StopCoroutine(actionCooldownCoroutine);

        actionCooldownCoroutine = StartCoroutine(HandleCoolDownCoroutine(coolDownTime));
    }

    private IEnumerator HandleCoolDownCoroutine(float coolDownTime)
    {
        isLocked = true;
        RefreshVisibility();

        yield return new WaitForSeconds(coolDownTime);

        isLocked = false;
        RefreshVisibility();

        actionCooldownCoroutine = null;
    }

    protected override void HideForJumpscare(CharacterName who, RoomName from)
    {
        isLocked = true;
        RefreshVisibility();
    }

    protected override void RefreshVisibility()
    {
        bool visible = ShouldBeVisible;
        bool usable = visible && !isLocked;

        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = usable;
        canvasGroup.blocksRaycasts = usable;

        if (button != null)
            button.interactable = usable;
    }
}
