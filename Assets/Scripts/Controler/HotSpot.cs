using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public abstract class HotSpot : OverLayObject
{
    [Header("Hotspot requirements")]
    [SerializeField] private bool startsAvailable = true;  

    protected Button button;
    private CanvasGroup canvasGroup;
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

    protected override void Start()
    {
        base.Start();

        player.OnActionPerformed += HandlePlayerAction;
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
    private void HandlePlayerAction(ActionType performedAction)
    {
        if (performedAction == null) return;
        if (button == null) return;

        if (actionCooldownCoroutine != null)
            StopCoroutine(actionCooldownCoroutine);

        actionCooldownCoroutine = StartCoroutine(HandlePlayerActionCoroutine(performedAction));
    }

    private IEnumerator HandlePlayerActionCoroutine(ActionType performedAction)
    {
        isLocked = true;
        RefreshState();

        yield return new WaitForSeconds(performedAction.HotspotDisableDuration);

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

    protected override void HideForJumpscare(CharacterName who, LocName from)
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

    protected override void OnDestroy()
    {
        if (player != null)
            player.OnActionPerformed -= HandlePlayerAction;

        base.OnDestroy();
    }
}
