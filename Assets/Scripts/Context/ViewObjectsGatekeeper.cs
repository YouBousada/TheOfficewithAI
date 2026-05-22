using UnityEngine;
public class ViewObjectsGatekeeper : MonoBehaviour
{
    [SerializeField] private GameContext gameContext;
    private ViewController activeViewController;
    private ViewController previouslyActiveViewData;

    public static ViewObjectsGatekeeper Instance {get; private set;}

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        foreach (ViewController viewController in gameContext.ViewControllers)
        {
            foreach (var obj in viewController.objectsInView)
            {
                obj.SetActive(false);
            }
        }

        if (!gameContext.TryGetView(gameContext.Player.CurrentView, out activeViewController))
        {
            // Debug.LogWarning($"{name}: No ViewController registered for starting view {gameContext.Player.CurrentView}.", this);
            return;
        }

        foreach (var obj in activeViewController.objectsInView)
            obj.SetActive(true);

        previouslyActiveViewData = activeViewController;
        gameContext.Player.OnViewChanged += RespondToViewChange;
        gameContext.AIMonitor.OnDeath += JumpscareHideObjects;
    }

    void RespondToViewChange(View newView)
    {
        previouslyActiveViewData = activeViewController;
        if (!gameContext.TryGetView(newView, out activeViewController))
        {
            // Debug.LogWarning($"{name}: No ViewController registered for view {newView}.", this);
            activeViewController = previouslyActiveViewData;
            return;
        }
        HideOldViewObjects();
        ShowNewViewObjects();
    }

    private void HideOldViewObjects()
    {
        foreach (var obj in previouslyActiveViewData.objectsInView)
        {
            obj.SetActive(false);
        }
    }

    private void ShowNewViewObjects()
    {
        foreach (var obj in activeViewController.objectsInView)
        {
            obj.SetActive(true);
        }
    }

    void JumpscareHideObjects(CharacterName killer, LocName location)
    {
        if (activeViewController == null) return;

        foreach (var obj in activeViewController.hideUponJumpscare)
            obj.SetActive(false);
    }

    void OnDestroy()
    {
        if (gameContext == null) return;

        gameContext.Player.OnViewChanged -= RespondToViewChange;
        gameContext.AIMonitor.OnDeath -= JumpscareHideObjects;
    }
}
