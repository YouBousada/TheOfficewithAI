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
        gameContext.Player.OnViewChanged += OnViewActivated;
        gameContext.AIMonitor.OnDeath += ReactToJumpscare;
    }
    protected abstract void RenderViewBasedOnWorldState();
    protected abstract void ReactToJumpscare_Concrete(CharacterName who, LocName from);

    protected void OnViewActivated(ViewName view)
    {
        if (view != thisViewName)
        {

            return;
        }

        RenderViewBasedOnWorldState();
    }
    public void ReactToJumpscare(CharacterName who, LocName from)
    {
        if (gameContext.Player.CurrentView != thisViewName) { return; }
        ReactToJumpscare_Concrete(who, from);
    }

    void OnDisable()
    {
        gameContext.Player.OnViewChanged -= OnViewActivated;
        gameContext.AIMonitor.OnDeath -= ReactToJumpscare;

    }


}
