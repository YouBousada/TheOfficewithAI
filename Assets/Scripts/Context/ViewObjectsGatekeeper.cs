using UnityEngine;
public class ViewObjectsGatekeeper : MonoBehaviour
{
    [SerializeField] private GameContext gameContext;
    private ViewController activeViewController;
    private ViewController previouslyActiveViewController;

    public static ViewObjectsGatekeeper Instance {get; private set;}

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        foreach (ViewController viewController in gameContext.ViewControllers)
        {
            foreach (var obj in viewController.ObjectsImmediatelyInView)
            {
                obj.SetActive(false);
            }
        }

        if (!gameContext.TryGetView(gameContext.Player.CurrentView, out activeViewController))
        {
            // Debug.LogWarning($"{name}: No ViewController registered for starting view {gameContext.Player.CurrentView}.", this);
            return;
        }

        foreach (var obj in activeViewController.ObjectsImmediatelyInView)
            obj.SetActive(true);

        previouslyActiveViewController = activeViewController;
        gameContext.Player.OnViewChanged += RespondToViewChange;
        gameContext.AIMonitor.OnDeath += JumpscareHideObjects;
    }

    void RespondToViewChange(View newView)
    {
        previouslyActiveViewController = activeViewController;
        if (!gameContext.TryGetView(newView, out activeViewController))
        {
            // Debug.LogWarning($"{name}: No ViewController registered for view {newView}.", this);
            activeViewController = previouslyActiveViewController;
            return;
        }
        HideOldViewObjects();
        ShowNewViewObjects();
    }

    private void HideOldViewObjects()
    {
        foreach (var obj in previouslyActiveViewController.ObjectsImmediatelyInView)
        {
            if (obj == null)
            {
                Debug.Log("Null object refrence in " + previouslyActiveViewController.GetViewName());
                continue;
            }
            
            obj.SetActive(false);
        }
    }

    private void ShowNewViewObjects()
    {
        foreach (var obj in activeViewController.ObjectsImmediatelyInView)
        {
            Debug.Log("Null object refrence in " + previouslyActiveViewController.GetViewName());
            obj.SetActive(true);
        }
    }

    void JumpscareHideObjects(CharacterName killer, LocName location)
    {
        if (activeViewController == null) return;

        foreach (var obj in activeViewController.HideUponJumpscare)
            obj.SetActive(false);
    }

    void OnDestroy()
    {
        if (gameContext == null) return;

        gameContext.Player.OnViewChanged -= RespondToViewChange;
        gameContext.AIMonitor.OnDeath -= JumpscareHideObjects;
    }
}
