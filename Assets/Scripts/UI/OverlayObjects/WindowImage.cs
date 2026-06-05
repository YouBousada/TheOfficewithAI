using UnityEngine;
using UnityEngine.UI;

public class WindowImage : OverLayObject, IAnimated, IJumpscareAffected
{
    [SerializeField] private RoomName thisWindowDoor;
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

        if (noOneAtThisWindow == null) Debug.LogWarning($"{name}: noOneAtThisWindow is not assigned.", this);
        if (bonnieAtThisWindow == null) Debug.LogWarning($"{name}: bonnieAtThisWindow is not assigned.", this);
        if (chicaAtThisWindow == null) Debug.LogWarning($"{name}: chicaAtThisWindow is not assigned.", this);
        if (foxyAtThisWindow == null) Debug.LogWarning($"{name}: foxyAtThisWindow is not assigned.", this);
        if (freddyAtThisWindow == null) Debug.LogWarning($"{name}: freddyAtThisWindow is not assigned.", this);
        if (jumpscareData == null) Debug.LogWarning($"{name}: jumpscareData is not assigned.", this);
    }

    void Reset()
    {
        AddActiveViewForWindowDoor();
    }

    protected override void Start()
    {
        base.Start();
        ClearWatcher();
    }

    public void UtilizeAnimationPlayer()
    {
        animPlayer = GetComponent<AnimationPlayer>();

        if (animPlayer == null)
        {
            animPlayer = gameObject.AddComponent<AnimationPlayer>();
        }
    }

    public void ReactToJumpscare(JumpscareEventData data)
    {
        CharacterName who = data.who;
        RoomName from = data.from;

        if (who == watcherAtThisWindow)
        {
            Image.sprite = noOneAtThisWindow;
            return;
        }

        if ((from == RoomName.EAST_DOOR && thisWindowDoor == RoomName.EAST_DOOR) || (from == RoomName.WEST_DOOR && thisWindowDoor == RoomName.WEST_DOOR))
        {
            if (watcherAtThisWindow == CharacterName.BONNIE)
                animPlayer.PlayAnimation(jumpscareData.FromHereBonnieAnimation, gameObject);
            else if (watcherAtThisWindow == CharacterName.CHICA)
                animPlayer.PlayAnimation(jumpscareData.FromHereChicaAnimation, gameObject);
            else if (watcherAtThisWindow == CharacterName.FOXY)
                animPlayer.PlayAnimation(jumpscareData.FromHereFoxyAnimation, gameObject);
            else if (watcherAtThisWindow == CharacterName.FREDDY)
                animPlayer.PlayAnimation(jumpscareData.FromHereFreddyAnimation, gameObject);
            return;
        }

        if ((from == RoomName.EAST_DOOR && thisWindowDoor == RoomName.WEST_DOOR) || (from == RoomName.WEST_DOOR && thisWindowDoor == RoomName.EAST_DOOR))
        {
            if (watcherAtThisWindow == CharacterName.BONNIE)
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorBonnieAnimation, gameObject);
            else if (watcherAtThisWindow == CharacterName.CHICA)
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorChicaAnimation, gameObject);
            else if (watcherAtThisWindow == CharacterName.FOXY)
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorFoxyAnimation, gameObject);
            else if (watcherAtThisWindow == CharacterName.FREDDY)
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorFreddyAnimation, gameObject);
        }
    }
    public void UpdateSpriteFromMoveEvent(LocationChangedData data)
    {
        CharacterName who = data.who;
        RoomName to = data.to;
        RoomName from = data.from;
        BehaviourName behaviour = data.behaviour;

        if (behaviour == BehaviourName.STARING_THROUGH_WINDOW && to == thisWindowDoor)
        {
            SetWatcher(who);
        }

        else if (from == thisWindowDoor)
        {
            ClearWatcher();
        }

    }

    public void UpdateSpriteFromJobswitch(JobSwitchedData data)
    {
        CharacterName who = data.who;
        JobName fromJob = data.fromJob;

        if (who == watcherAtThisWindow && fromJob == JobName.WINDOW_WATCHER)
        {
            ClearWatcher();
        }
    }


    private void SetWatcher(CharacterName who)
    {
        watcherAtThisWindow = who;

        Image.sprite = who switch
        {
            CharacterName.BONNIE => bonnieAtThisWindow,
            CharacterName.CHICA => chicaAtThisWindow,
            CharacterName.FOXY => foxyAtThisWindow,
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
        SetVisible(true);
    }

    protected override void ExitActiveView()
    {
        SetVisible(false);
    }

    protected override void HideForJumpscare(CharacterName who, RoomName from)
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
        if (thisWindowDoor == RoomName.WEST_DOOR)
            AddActiveViewIfMissing(ViewName.LEFT_DOOR);
        else if (thisWindowDoor == RoomName.EAST_DOOR)
            AddActiveViewIfMissing(ViewName.RIGHT_DOOR);
    }

}
