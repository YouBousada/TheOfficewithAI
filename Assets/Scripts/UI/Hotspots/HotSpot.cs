using System.Collections;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public abstract class HotSpot : OverLayObject
{
    [Header("Hotspot requirements")]
    [SerializeField] private bool startsAvailable = true;  
    protected Button button;
    protected CanvasGroup canvasGroup;
    private Coroutine actionCooldownCoroutine;
    protected bool isAvailableFromWorldState {get; private set;}
    protected bool isLocked {get; private set;}

    public bool CanUseHotspot => IsInActiveView &&
                                 isAvailableFromWorldState &&
                                 !isLocked;
    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        isAvailableFromWorldState = startsAvailable;
    }

    public void ActivateHotspot()
    {
        isAvailableFromWorldState = true;
        RefreshState();
    }

    public void DeactivateHotspot()
    {
        isAvailableFromWorldState = false;
        RefreshState();
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
        RefreshState();

        yield return new WaitForSeconds(coolDownTime);

        isLocked = false;
        RefreshState();

        actionCooldownCoroutine = null;
    }

    protected override void EnterActiveView()
    {
        RefreshState();
    }

    protected override void ExitActiveView()
    {
        RefreshState();
    }

    protected override void HideForJumpscare(CharacterName who, RoomName from)
    {
        isLocked = true;
        RefreshState();
    }

    private void RefreshState()
    {
        bool visible = IsInActiveView && isAvailableFromWorldState;
        bool usable = visible && !isLocked;

        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = usable;
        canvasGroup.blocksRaycasts = usable;

        if (button != null)
            button.interactable = usable;
    }
}
