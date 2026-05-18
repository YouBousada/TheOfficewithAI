using UnityEngine;
public class WhatsInViewManager : MonoBehaviour
{
    public World world;
    private ViewController activeViewController;
    private ViewController previouslyActiveViewData;
    void Start()
    {
        foreach (var viewData in world.gameViews)
            foreach (var obj in viewData.objectsInView)
                obj.SetActive(false);


        activeViewController = world.viewDictionary[world.player.currentView];
        foreach (var obj in activeViewController.objectsInView)
            obj.SetActive(true);

        previouslyActiveViewData = activeViewController;
        world.playerMonitor.OnViewChanged += RespondToViewChange;
        world.aiMonitor.OnDeath += JumpscareHideObjects;
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
        foreach (var obj in activeViewController.objectsInView)
        {
            if(activeViewController.hideUponJumpscare.Contains(obj))
            {
                obj.SetActive(false);
            }
        }
    }
}
