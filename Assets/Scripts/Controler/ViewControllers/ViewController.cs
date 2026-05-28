using UnityEngine;
using UnityEngine.UI;

public abstract class ViewController : MonoBehaviour, IAnimated, IJumpscareAffected
{
    protected AnimationPlayer animPlayer;

    [Header("ViewController requirements")]
    [SerializeField] protected Player player;
    [SerializeField] protected AIEvents aiMonitor;
    [SerializeField] protected PuzzleMaster puzzleMaster;
    [SerializeField] protected Office office;
    [SerializeField] protected Image backgroundImage;
    [SerializeField] protected ViewName thisViewName;

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

        if (player == null)        Debug.LogWarning($"{name}: player is not assigned.", this);
        if (aiMonitor == null)     Debug.LogWarning($"{name}: aiMonitor is not assigned.", this);
        if (puzzleMaster == null)  Debug.LogWarning($"{name}: puzzleMaster is not assigned.", this);
        if (office == null)        Debug.LogWarning($"{name}: office is not assigned.", this);
        if (backgroundImage == null) Debug.LogWarning($"{name}: backgroundImage is not assigned.", this);

        if (puzzleMaster != null)
            puzzleMaster.OnRoomStateChanged += RenderViewBasedOnWorldState;
    }

    void Start()
    {
        if (player == null || aiMonitor == null) return;
        OnViewActivated(player.CurrentView);
        player.OnViewChanged += OnViewActivated;
        aiMonitor.OnDeath += ReactToJumpscare;
    }

    protected abstract void RenderViewBasedOnWorldState_Concrete();
    protected abstract void ReactToJumpscare_Concrete(CharacterName who, LocName from);

    private void RenderViewBasedOnWorldState()
    {
        if(player.CurrentView != thisViewName)
        {
            return;
        }

        RenderViewBasedOnWorldState_Concrete();
    }
    private void OnViewActivated(ViewName view)
    {
        if (view != thisViewName)
            return;

        RenderViewBasedOnWorldState_Concrete();
    }

    public void ReactToJumpscare(CharacterName who, LocName from)
    {
        if (player.CurrentView != thisViewName) return;
        ReactToJumpscare_Concrete(who, from);
    }

    void OnDisable()
    {
        if (player != null)    player.OnViewChanged -= OnViewActivated;
        if (aiMonitor != null) aiMonitor.OnDeath -= ReactToJumpscare;
        if (puzzleMaster != null) puzzleMaster.OnRoomStateChanged -= RenderViewBasedOnWorldState;
    }
}
