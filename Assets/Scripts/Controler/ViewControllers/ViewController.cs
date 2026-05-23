using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class ViewController : MonoBehaviour, IAnimated, IJumpscareAffected
{
    protected AnimationPlayer animPlayer;

    [Header("Context")]
    [SerializeField] protected GameContext gameContext;
    [SerializeField] protected Image backgroundImage;

    [Header("Info about view")]
    [SerializeField] protected View thisViewName;
    [SerializeField] private List<GameObject> objectsImmediatelyInView = new();
    [SerializeField] private List<GameObject> hideUponJumpscare = new();
    public IReadOnlyList<GameObject> ObjectsImmediatelyInView => objectsImmediatelyInView;
    public IReadOnlyList<GameObject> HideUponJumpscare => hideUponJumpscare;

    public View GetViewName()
    {
        return thisViewName;
    }

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
        
        gameContext.RegisterView(thisViewName, this);
    }

    void Start()
    {

        OnViewActivated(gameContext.Player.CurrentView);

        gameContext.Player.OnActionPerformed += RespondToAction_Concrete;
        gameContext.Player.OnViewChanged += OnViewActivated;
        gameContext.AIMonitor.OnDeath += ReactToJumpscare;
    }

    protected abstract void RespondToAction_Concrete(PlayerAction action);
    protected abstract void OnViewActivated_Concrete();
    protected abstract void ReactToJumpscare_Concrete(CharacterName who, LocName from);


    protected void OnPlayerAction(PlayerAction action)
    {
        if(action.AllowedView != thisViewName)
        {
            Debug.Log("PlayerAction " + action.ActionName + " was performed in an illegal view! Incorrect ObjectsInView Assignment?");
            return;
        }

        RespondToAction_Concrete(action);
    }

    protected void OnViewActivated(View view)
    {
        if (view != thisViewName)
        {

            return;
        }

        OnViewActivated_Concrete();
    }
    public void ReactToJumpscare(CharacterName who, LocName from)
    {
        if (gameContext.Player.CurrentView != thisViewName) { return; }
        ReactToJumpscare_Concrete(who, from);
    }

    void OnDisable()
    {
        gameContext.Player.OnActionPerformed -= RespondToAction_Concrete;
        gameContext.Player.OnViewChanged -= OnViewActivated;
        gameContext.AIMonitor.OnDeath -= ReactToJumpscare;

    }

    void OnDestroy()
    {
        if (gameContext != null)
            gameContext.UnregisterView(thisViewName, this);
    }


}
