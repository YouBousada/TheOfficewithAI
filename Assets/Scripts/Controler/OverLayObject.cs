using System.Collections.Generic;
using UnityEngine;

public abstract class OverLayObject : MonoBehaviour
{
    [Header("OverLayObject requirements")]
    [SerializeField] protected Player player;
    [SerializeField] protected AiMonitor aiMonitor;
    [SerializeField] private List<ViewName> viewsICanBeActiveIn = new();
    [SerializeField] protected bool hideUponJumpscare = true;
    private bool hasHandledViewChange;

    protected bool IsInActiveView { get; private set; }

    protected void AddActiveViewIfMissing(ViewName view)
    {
        if (!viewsICanBeActiveIn.Contains(view))
            viewsICanBeActiveIn.Add(view);
    }

    protected virtual void Start()
    {
        if (player == null)
        {
            Debug.LogWarning($"{name} is missing a Player reference.", this);
            return;
        }

        if (viewsICanBeActiveIn.Count == 0)
        {
            Debug.LogWarning($"{name} is not set to be active in any view.", this);
            return;
        }

        player.OnViewChanged += HandleViewChanged;

        if (aiMonitor != null)
            aiMonitor.OnDeath += HandleJumpscare;

        HandleViewChanged(player.CurrentView);
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
        if (player != null)
            player.OnViewChanged -= HandleViewChanged;

        if (aiMonitor != null)
            aiMonitor.OnDeath -= HandleJumpscare;
    }
}
