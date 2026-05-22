using UnityEngine;
public class ViewObjectsGatekeeper : MonoBehaviour
{
    [SerializeField] private GameContext world;
    private ViewController activeViewController;
    private ViewController previouslyActiveViewData;

    public static ViewObjectsGatekeeper Instance {get; private set;}

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        foreach (ViewController viewController in world.ViewControllers)
        {
            foreach (var obj in viewController.objectsInView)
            {
                obj.SetActive(false);
            }
        }

        if (!world.TryGetView(world.Player.CurrentView, out activeViewController))
        {
            // Debug.LogWarning($"{name}: No ViewController registered for starting view {world.Player.CurrentView}.", this);
            return;
        }

        foreach (var obj in activeViewController.objectsInView)
            obj.SetActive(true);

        previouslyActiveViewData = activeViewController;
        world.Player.OnViewChanged += RespondToViewChange;
        world.AIMonitor.OnDeath += JumpscareHideObjects;
    }

    void RespondToViewChange(View newView)
    {
        previouslyActiveViewData = activeViewController;
        if (!world.TryGetView(newView, out activeViewController))
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
        if (world == null) return;

        world.Player.OnViewChanged -= RespondToViewChange;
        world.AIMonitor.OnDeath -= JumpscareHideObjects;
    }
}
