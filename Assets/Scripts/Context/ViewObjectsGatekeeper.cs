using UnityEngine;
public class ViewObjectsGatekeeper : MonoBehaviour
{
    [SerializeField] private World world;
    private ViewController activeViewController;
    private ViewController previouslyActiveViewData;

    public static ViewObjectsGatekeeper Instance {get; private set;}

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        foreach (ViewController viewController in world.viewDictionary.Values)
        {
            foreach (var obj in viewController.objectsInView)
            {
                obj.SetActive(false);
            }
        }

        activeViewController = world.viewDictionary[world.Player.CurrentView];

        foreach (var obj in activeViewController.objectsInView)
            obj.SetActive(true);

        previouslyActiveViewData = activeViewController;
        world.Player.OnViewChanged += RespondToViewChange;
        world.AIMonitor.OnDeath += JumpscareHideObjects;
    }

    void RespondToViewChange(View newView)
    {
        previouslyActiveViewData = activeViewController;
        activeViewController = world.viewDictionary[newView];
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
        foreach (var obj in activeViewController.hideUponJumpscare)
            obj.SetActive(false);
    }
}
