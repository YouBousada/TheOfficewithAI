using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewManager : MonoBehaviour
{
    [SerializeField] private List<ViewController> viewPrefabs;
    [SerializeField] private Transform viewContainer;
    [SerializeField] PlayerConfig playerConfig;
    [SerializeField] protected Image backgroundImage;

    private readonly Dictionary<ViewName, ViewController> views = new();
    private ViewController activeView;

    void Awake()
    {
        foreach (var prefab in viewPrefabs)
        {
            var instance = Instantiate(prefab, viewContainer);
            instance.backgroundImage = backgroundImage;
            instance.gameObject.SetActive(false);
            views[instance.ViewName] = instance;
        }
    }
    void Start()
    {
        SwitchTo(playerConfig.startingview);
    }

    public void SwitchTo(ViewName view)
    {
        if (activeView != null) activeView.gameObject.SetActive(false);

        if (!views.TryGetValue(view, out ViewController target))
        {
            Debug.LogWarning($"ViewManager: no view registered for {view}");
            return;
        }

        activeView = target;
        activeView.gameObject.SetActive(true);
    }

    public void UpdateView()
    {
        activeView.RenderViewBasedOnWorldState();
    }

    public void ReactToJumpscare(JumpscareEventData data)
    {
        activeView?.ReactToJumpscare(data);
    }
}
