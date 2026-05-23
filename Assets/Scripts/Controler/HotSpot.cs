using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HotSpot : OverLayObject
{
    [SerializeField] private View to;
    [SerializeField] private PlayerAction action;
    [SerializeField] private bool startsAvailable = true;

    private Button button;
    private CanvasGroup canvasGroup;
    private Coroutine actionCooldownCoroutine;
    private bool isAvailableFromWorldState;
    private bool isActionLocked;
    private bool isJumpscareLocked;

    public bool CanUseHotspot => IsInActiveView &&
                                 isAvailableFromWorldState &&
                                 !isActionLocked &&
                                 !isJumpscareLocked;

    private void Awake()
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

        gameContext.Player.OnActionPerformed += HandlePlayerAction;
    }

    public void InvokeViewChange()
    {
        if (!CanUseHotspot) return;

        gameContext.Player.SetView(to);
    }

    public void InvokeAction()
    {
        if (!CanUseHotspot) return;
        if (action == null)
        {
            Debug.LogWarning($"{name} cannot perform an action because no PlayerAction is assigned.", this);
            return;
        }

        gameContext.Player.PerformAction(action);
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

    private void HandlePlayerAction(PlayerAction performedAction)
    {
        if (performedAction == null) return;
        if (button == null) return;

        if (actionCooldownCoroutine != null)
            StopCoroutine(actionCooldownCoroutine);

        actionCooldownCoroutine = StartCoroutine(HandlePlayerActionCoroutine(performedAction));
    }

    private IEnumerator HandlePlayerActionCoroutine(PlayerAction performedAction)
    {
        isActionLocked = true;
        RefreshState();

        yield return new WaitForSeconds(performedAction.HotspotDisableDuration);

        isActionLocked = false;
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
        isJumpscareLocked = true;
        RefreshState();
    }

    private void RefreshState()
    {
        bool visible = IsInActiveView && isAvailableFromWorldState && !isJumpscareLocked;
        bool usable = visible && !isActionLocked;

        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = usable;
        canvasGroup.blocksRaycasts = usable;

        if (button != null)
            button.interactable = usable;
    }

    protected override void OnDestroy()
    {
        if (gameContext != null)
            gameContext.Player.OnActionPerformed -= HandlePlayerAction;

        base.OnDestroy();
    }
}
