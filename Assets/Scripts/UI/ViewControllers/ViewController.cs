using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ViewController : MonoBehaviour, IAnimated
{
    protected AnimationPlayer animPlayer;

    [Header("ViewController requirements")]
    public Image backgroundImage;
    [field: SerializeField] public ViewName ViewName { get; private set; }
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] protected CurrentView currentView;

    public void UtilizeAnimationPlayer()
    {
        animPlayer = GetComponent<AnimationPlayer>();

        if (animPlayer == null)
            animPlayer = gameObject.AddComponent<AnimationPlayer>();
    }

    protected virtual void Awake()
    {
        UtilizeAnimationPlayer();
    }

    protected virtual void OnEnable()
    {
        RenderViewBasedOnWorldState();
    }

    public abstract void RenderViewBasedOnWorldState();
    public abstract void ReactToJumpscare(JumpscareEventData jsData);
}
