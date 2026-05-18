using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class ViewController : MonoBehaviour, IAnimated, IJumpscareAffected
{
    protected AnimationPlayer animPlayer;

    [Header("Context")]
    public World world;
    public Image backgroundImage;

    [Header("Info about view")]
    public View thisViewName;
    public List<GameObject> objectsInView = new();
    public List<GameObject> hideUponJumpscare = new();

    public void UtilizeAnimationPlayer()
    {
        animPlayer = GetComponent<AnimationPlayer>();

        if (animPlayer == null)
        {
            animPlayer = gameObject.AddComponent<AnimationPlayer>();
        }
    }

    protected virtual void Awake()
    {
        UtilizeAnimationPlayer();
    }

    void Start()
    {
        OnViewActivated(world.player.currentView);

        world.playerMonitor.OnActionPerformed += RespondToAction;
        world.playerMonitor.OnViewChanged += OnViewActivated;
        world.aiMonitor.OnDeath += ReactToJumpscare;
    }

    protected abstract void RespondToAction(Action action);
    protected abstract void OnViewActivated_Concrete();
    protected abstract void ReactToJumpscare_Concrete(CharacterName who, LocName from);


    protected void OnViewActivated(View view)
    {
        if (view != thisViewName) { return; }
        OnViewActivated_Concrete();
    }

    public void ReactToJumpscare(CharacterName who, LocName from)
    {
        if (world.player.currentView != thisViewName) { return; }
        ReactToJumpscare_Concrete(who, from);
    }

    void OnDisable()
    {
        world.playerMonitor.OnActionPerformed -= RespondToAction;
        world.playerMonitor.OnViewChanged -= OnViewActivated;
        world.aiMonitor.OnDeath -= ReactToJumpscare;

    }


}