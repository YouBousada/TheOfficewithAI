using UnityEngine;
using UnityEngine.UI;

public class WindowImage : OverLayObject, IAnimated, IJumpscareAffected
{
    [SerializeField] private LocName thisWindowDoor;
    private Image Image;
    private AnimationPlayer animPlayer;
    private CharacterName watcherAtThisWindow = CharacterName.NONE;

    [Header("Idle Window Sprites")]
    [SerializeField] private Sprite noOneAtThisWindow;
    [SerializeField] private Sprite bonnieAtThisWindow;
    [SerializeField] private Sprite chicaAtThisWindow;
    [SerializeField] private Sprite foxyAtThisWindow;
    [SerializeField] private Sprite freddyAtThisWindow;

    [Header("JumpscareData")]
    [SerializeField] private WindowJumpscareData jumpscareData;

    void Awake()
    {
        AddActiveViewForWindowDoor();
        UtilizeAnimationPlayer();
        Image = GetComponent<Image>();
    }

    void Reset()
    {
        AddActiveViewForWindowDoor();
    }

    protected override void Start()
    {
        base.Start();
        
        DoSubscribing();
        RefreshWindowSprite();
    }

    public void UtilizeAnimationPlayer()
    {
        animPlayer = GetComponent<AnimationPlayer>();

        if (animPlayer == null)
        {
            animPlayer = gameObject.AddComponent<AnimationPlayer>();
        }
    }

    public void ReactToJumpscare(CharacterName who, LocName from)
    {
        if(who == watcherAtThisWindow)
        {
            Image.sprite = noOneAtThisWindow;
            return;
        }
        
        if((from == LocName.EAST_DOOR && thisWindowDoor == LocName.EAST_DOOR) || (from == LocName.WEST_DOOR && thisWindowDoor == LocName.WEST_DOOR))
        {
            if(watcherAtThisWindow == CharacterName.BONNIE)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereBonnieAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.CHICA)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereChicaAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.FOXY)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereFoxyAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.FREDDY)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereFreddyAnimation, gameObject);
            }
            return;
        }

        if((from == LocName.EAST_DOOR && thisWindowDoor == LocName.WEST_DOOR) || (from == LocName.WEST_DOOR && thisWindowDoor == LocName.EAST_DOOR))
        {
            if(watcherAtThisWindow == CharacterName.BONNIE)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorBonnieAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.CHICA)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorChicaAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.FOXY)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorFoxyAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.FREDDY)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorFreddyAnimation, gameObject);
            }
            return;
        }
        
    }

    private void DoSubscribing()
    {
        gameContext.AIMonitor.OnInitialization += RespondToInitialization;
        gameContext.AIMonitor.OnLocationChanged += RespondToMovement;
        gameContext.AIMonitor.OnJobSwitched += RespondToJobSwitch;
        gameContext.AIMonitor.OnDeath += ReactToJumpscare;
    }

    protected override void OnDestroy()
    {
        if (gameContext != null)
        {
            gameContext.AIMonitor.OnInitialization -= RespondToInitialization;
            gameContext.AIMonitor.OnLocationChanged -= RespondToMovement;
            gameContext.AIMonitor.OnJobSwitched -= RespondToJobSwitch;
            gameContext.AIMonitor.OnDeath -= ReactToJumpscare;
        }

        base.OnDestroy();
    }

    void RespondToInitialization(CharacterName who, LocName location, BehaviourName behaviour, JobName job)
    {
        if (IsInActiveView)
            RefreshWindowSprite();
    }

    void RespondToJobSwitch(CharacterName who, JobName fromJob, JobName toJob)
    {
        if (IsInActiveView)
            RefreshWindowSprite();
    }

    void RespondToMovement(CharacterName who, LocName from, LocName to)
    {
        if (IsInActiveView || from == thisWindowDoor || to == thisWindowDoor)
            RefreshWindowSprite();
    }

    private void RefreshWindowSprite()
    {
        foreach (Animatronic animatronic in gameContext.Animatronics)
        {
            if (animatronic.GetCurrentLocation() != thisWindowDoor) continue;
            if (animatronic.GetCurrentJobName() != JobName.WINDOW_WATCHER) continue;

            SetWatcher(animatronic.GetCharacterName());
            return;
        }

        ClearWatcher();
    }

    private void SetWatcher(CharacterName who)
    {
        watcherAtThisWindow = who;

        Image.sprite = who switch
        {
            CharacterName.BONNIE => bonnieAtThisWindow,
            CharacterName.CHICA  => chicaAtThisWindow,
            CharacterName.FOXY   => foxyAtThisWindow,
            CharacterName.FREDDY => freddyAtThisWindow,
            _ => noOneAtThisWindow
        };
    }

    private void ClearWatcher()
    {
        watcherAtThisWindow = CharacterName.NONE;
        Image.sprite = noOneAtThisWindow;
    }

    protected override void EnterActiveView()
    {
        RefreshWindowSprite();
        SetVisible(true);
    }

    protected override void ExitActiveView()
    {
        SetVisible(false);
    }

    protected override void HideForJumpscare(CharacterName who, LocName from)
    {
        SetVisible(!hideUponJumpscare);
    }

    private void SetVisible(bool visible)
    {
        if (Image != null)
            Image.enabled = visible;
    }

    private void AddActiveViewForWindowDoor()
    {
        if (thisWindowDoor == LocName.WEST_DOOR)
            AddActiveViewIfMissing(ViewName.LEFT_DOOR);
        else if (thisWindowDoor == LocName.EAST_DOOR)
            AddActiveViewIfMissing(ViewName.RIGHT_DOOR);
    }

}
