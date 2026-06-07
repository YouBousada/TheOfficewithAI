using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ViewChangedListener))]
public abstract class OverLayObject : MonoBehaviour
{
    [Header("OverLayObject requirements")]
    [SerializeField] private List<ViewName> viewsICanBeActiveIn = new();
    [SerializeField] protected bool hideUponJumpscare = true;
    [SerializeField] private bool startsAvailable = true;
    [SerializeField] PlayerConfig playerConfig;
    private bool hasHandledViewChange;

    public bool isAvailableFromWorldState { get; private set; }
    protected bool IsInActiveView { get; private set; }
    protected bool ShouldBeVisible => IsInActiveView && isAvailableFromWorldState;

    protected Image image;

    protected virtual void Awake()
    {
        image = GetComponent<Image>();
        isAvailableFromWorldState = startsAvailable;
    }
    public void SetVisible(bool visible)
    {
        if (image != null)
            image.enabled = visible;
    }

    public void Activate()
    {
        isAvailableFromWorldState = true;
        RefreshVisibility();
    }

    public void Deactivate()
    {
        isAvailableFromWorldState = false;
        RefreshVisibility();
    }

    protected void AddActiveViewIfMissing(ViewName view)
    {
        if (!viewsICanBeActiveIn.Contains(view))
            viewsICanBeActiveIn.Add(view);
    }

    protected virtual void Start()
    {

        if (viewsICanBeActiveIn.Count == 0)
        {
            Debug.LogWarning($"{name} is not set to be active in any view.", this);
            return;
        }

        HandleViewChanged(playerConfig.startingview);
    }

    public void HandleViewChanged(ViewName newView)
    {
        bool shouldBeActive = viewsICanBeActiveIn.Contains(newView);

        if (hasHandledViewChange && shouldBeActive == IsInActiveView)
            return;

        hasHandledViewChange = true;
        IsInActiveView = shouldBeActive;

        RefreshVisibility();
    }

    public void HandleJumpscare(JumpscareEventData jsData)
    {
        CharacterName who = jsData.who;
        RoomName from = jsData.from;

        if (!hideUponJumpscare) return;
        HideForJumpscare(who, from);
    }

    protected virtual void RefreshVisibility() => SetVisible(ShouldBeVisible);
    protected abstract void HideForJumpscare(CharacterName who, RoomName from);
}
