using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ViewController : MonoBehaviour, IAnimated
{
    protected AnimationPlayer animPlayer;

    [Header("ViewController requirements")]
    // [SerializeField] protected PuzzleMaster puzzleMaster;
    // [SerializeField] protected Office office;
    public Image backgroundImage;
    [SerializeField] protected ViewName thisViewName;
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] protected CurrentView currentView;

    public ViewName GetViewName() => thisViewName;

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

    void Start()
    {
        RenderViewBasedOnWorldState_Concrete();
    }

    protected abstract void RenderViewBasedOnWorldState_Concrete();
    public abstract void ReactToJumpscare(JumpscareEventData jsData);
}
