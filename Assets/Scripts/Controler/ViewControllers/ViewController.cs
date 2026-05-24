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
    [SerializeField] protected ViewName thisViewName;

    public ViewName GetViewName()
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
        
    }

    void Start()
    {

        OnViewActivated(gameContext.Player.CurrentView);

        // gameContext.Player.OnActionPerformed += RespondToAction_Concrete;
        gameContext.Player.OnViewChanged += OnViewActivated;
        gameContext.AIMonitor.OnDeath += ReactToJumpscare;
    }

    // protected abstract void RespondToAction_Concrete(ActionType action);
    protected abstract void OnViewActivated_Concrete();
    protected abstract void ReactToJumpscare_Concrete(CharacterName who, LocName from);


    // protected void OnPlayerAction(ActionType action)
    // {
    //     RespondToAction_Concrete(action);
    // }

    protected void OnViewActivated(ViewName view)
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
        // gameContext.Player.OnActionPerformed -= RespondToAction_Concrete;
        gameContext.Player.OnViewChanged -= OnViewActivated;
        gameContext.AIMonitor.OnDeath -= ReactToJumpscare;

    }


}
