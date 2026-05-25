using UnityEngine;
using UnityEngine.UI;

public class WindowImage : OverLayObject, IAnimated, IJumpscareAffected
{
    [SerializeField] private LocName thisWindowDoor;
    private Image Image;
    private AnimationPlayer animPlayer;
    private CharacterName watcherAtThisWindow = CharacterName.None;

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
        
        if((from == LocName.EastDoor && thisWindowDoor == LocName.EastDoor) || (from == LocName.WestDoor && thisWindowDoor == LocName.WestDoor))
        {
            if(watcherAtThisWindow == CharacterName.Bonnie)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereBonnieAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.Chica)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereChicaAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.Foxy)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereFoxyAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.Freddy)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereFreddyAnimation, gameObject);
            }
            return;
        }

        if((from == LocName.EastDoor && thisWindowDoor == LocName.WestDoor) || (from == LocName.WestDoor && thisWindowDoor == LocName.EastDoor))
        {
            if(watcherAtThisWindow == CharacterName.Bonnie)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorBonnieAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.Chica)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorChicaAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.Foxy)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorFoxyAnimation, gameObject);
            }
            else if(watcherAtThisWindow == CharacterName.Freddy)
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
            if (animatronic.GetCurrentJobName() != JobName.WindowWatcher) continue;

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
            CharacterName.Bonnie => bonnieAtThisWindow,
            CharacterName.Chica  => chicaAtThisWindow,
            CharacterName.Foxy   => foxyAtThisWindow,
            CharacterName.Freddy => freddyAtThisWindow,
            _ => noOneAtThisWindow
        };
    }

    private void ClearWatcher()
    {
        watcherAtThisWindow = CharacterName.None;
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
        if (thisWindowDoor == LocName.WestDoor)
            AddActiveViewIfMissing(ViewName.LeftDoor);
        else if (thisWindowDoor == LocName.EastDoor)
            AddActiveViewIfMissing(ViewName.RightDoor);
    }

}
