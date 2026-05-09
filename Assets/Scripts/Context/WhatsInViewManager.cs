using UnityEngine;
public class WhatsInViewManager : MonoBehaviour
{
    public World world;
    private ViewData currentViewData;
    private ViewData previousViewData;
    void Start()
    {
        foreach (var viewData in world.gameViews)
            foreach (var obj in viewData.objectsInView)
                obj.SetActive(false);


        currentViewData = world.viewDictionary[world.player.currentView];
        foreach (var obj in currentViewData.objectsInView)
            obj.SetActive(true);

        previousViewData = currentViewData;
        world.playerMonitor.OnViewChanged += RespondToViewChange;
        world.aiMonitor.OnDeath += JumpscareHideObjects;
    }

    void RespondToViewChange(View newView, float time)
    {
        previousViewData = currentViewData;
        currentViewData = world.viewDictionary[newView];
        HideOldViewObjects();
        ShowNewViewObjects();
    }

    private void HideOldViewObjects()
    {
        foreach (var obj in previousViewData.objectsInView)
        {
            obj.SetActive(false);
        }
    }

    private void ShowNewViewObjects()
    {
        foreach (var obj in currentViewData.objectsInView)
        {
            obj.SetActive(true);
        }
    }

    void JumpscareHideObjects(CharacterName killer, LocName location)
    {   
        foreach (var obj in currentViewData.objectsInView)
        {
            if(currentViewData.hideUponJumpscare.Contains(obj))
            {
                obj.SetActive(false);
            }
        }
    }
}
