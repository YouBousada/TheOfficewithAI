using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class OverLayObject : MonoBehaviour
{
    [Header("OverLayObject requirements")]
    [SerializeField] protected GameContext gameContext;
    [SerializeField] private List<ViewName> viewsICanBeActiveIn = new();
    [SerializeField] private bool hideUponJumpscare = true;
    private bool hasHandledViewChange;

    protected bool IsInActiveView { get; private set; }

    protected void AddActiveViewIfMissing(ViewName view)
    {
        if (!viewsICanBeActiveIn.Contains(view))
            viewsICanBeActiveIn.Add(view);
    }

    protected virtual void Start()
    {
        if (gameContext == null)
            gameContext = GameContext.Instance;

        if (gameContext == null || gameContext.Player == null)
        {
            Debug.LogWarning($"{name} cannot initialize overlay behavior because GameContext or Player is missing.", this);
            return;
        }

        if (viewsICanBeActiveIn.Count == 0)
        {
            Debug.LogWarning($"{name} Is not set to be active in any view!.", this);
            return;
        }

        gameContext.Player.OnViewChanged += HandleViewChanged;

        if (gameContext.AIMonitor != null)
            gameContext.AIMonitor.OnDeath += HandleJumpscare;

        HandleViewChanged(gameContext.Player.CurrentView);
    }

    private void HandleViewChanged(ViewName newView)
    {
        bool shouldBeActive = viewsICanBeActiveIn.Contains(newView);

        if (hasHandledViewChange && shouldBeActive == IsInActiveView)
            return;

        hasHandledViewChange = true;
        IsInActiveView = shouldBeActive;

        if (IsInActiveView)
            EnterActiveView();
        else
            ExitActiveView();
    }

    private void HandleJumpscare(CharacterName who, LocName from)
    {
        if (!hideUponJumpscare) return;

        HideForJumpscare(who, from);
    }

    protected abstract void EnterActiveView();
    protected abstract void ExitActiveView();
    protected abstract void HideForJumpscare(CharacterName who, LocName from);

    protected virtual void OnDestroy()
    {
        if (gameContext == null) return;

        if (gameContext.Player != null)
            gameContext.Player.OnViewChanged -= HandleViewChanged;

        if (gameContext.AIMonitor != null)
            gameContext.AIMonitor.OnDeath -= HandleJumpscare;
    }
}
