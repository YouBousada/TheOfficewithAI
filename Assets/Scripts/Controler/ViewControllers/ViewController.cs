using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class ViewController : MonoBehaviour, IAnimated, IJumpscareAffected
{
    protected AnimationPlayer animPlayer;

    [Header("Context")]
    [SerializeField] protected World world;
    [SerializeField] protected Image backgroundImage;

    [Header("Info about view")]
    [SerializeField] private View thisViewName;
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
        world.viewDictionary.Add(thisViewName, this);
    }

    void Start()
    {

        OnViewActivated(world.Player.CurrentView);

        world.Player.OnActionPerformed += RespondToAction;
        world.Player.OnViewChanged += OnViewActivated;
        world.AIMonitor.OnDeath += ReactToJumpscare;
    }

    protected abstract void RespondToAction(Action action, float time);
    protected abstract void OnViewActivated_Concrete();
    protected abstract void ReactToJumpscare_Concrete(CharacterName who, LocName from);


    protected void OnViewActivated(View view)
    {
        if (view != thisViewName) { return; }
        OnViewActivated_Concrete();
    }

    public void ReactToJumpscare(CharacterName who, LocName from)
    {
        if (world.Player.CurrentView != thisViewName) { return; }
        ReactToJumpscare_Concrete(who, from);
    }

    void OnDisable()
    {
        world.Player.OnActionPerformed -= RespondToAction;
        world.Player.OnViewChanged -= OnViewActivated;
        world.AIMonitor.OnDeath -= ReactToJumpscare;

    }


}