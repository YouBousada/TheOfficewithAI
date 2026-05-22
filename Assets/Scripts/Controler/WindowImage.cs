using UnityEngine;
using UnityEngine.UI;

public class WindowImage : MonoBehaviour, IAnimated, IJumpscareAffected
{
    [SerializeField] private GameContext world;
    [SerializeField] private LocName thisWindowDoor;
    private Image Image;
    private AnimationPlayer animPlayer;
    private CharacterName watcherAtThisWindow = CharacterName.None;
    private bool started = false;

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
        UtilizeAnimationPlayer();
        Image = GetComponent<Image>();
    }

    void OnEnable()
    {
        if (!started)
        {
            return;
        }

        DoSubscribing();
        SeeIfAWindowWatcherIsHere();
    }

    void Start()
    {
        started = true;
        
        DoSubscribing();
        SeeIfAWindowWatcherIsHere();
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
        world.AIMonitor.OnLocationChanged += RespondToMovement;
        world.AIMonitor.OnJobSwitched += RespondToJobSwitch;
        world.AIMonitor.OnDeath += ReactToJumpscare;
    }

    private void SeeIfAWindowWatcherIsHere()
    {
        if (IsWindowWatcherAtDoor(CharacterName.Bonnie))
        {
            RespondToMovement(CharacterName.Bonnie, LocName.None, thisWindowDoor);
            // Debug.Log("WindowImage: Bonnie found at his window on enable");
        }
        else if (IsWindowWatcherAtDoor(CharacterName.Chica))
        {
            RespondToMovement(CharacterName.Chica, LocName.None, thisWindowDoor);
            // Debug.Log("WindowImage: Chica found at her window on enable");
        }
        else if (IsWindowWatcherAtDoor(CharacterName.Foxy))
        {
            RespondToMovement(CharacterName.Foxy, LocName.None, thisWindowDoor);
            // Debug.Log("WindowImage: Foxy found at his window on enable");
        }
        else if (IsWindowWatcherAtDoor(CharacterName.Freddy))
        {
            RespondToMovement(CharacterName.Freddy, LocName.None, thisWindowDoor);
            // Debug.Log("WindowImage: Freddy found at his window on enable");
        }
        else
        {
            watcherAtThisWindow = CharacterName.None;
            Image.sprite = noOneAtThisWindow;
            // Debug.Log("WindowImage: No one found at this window on enable");
        }
    }

    void OnDisable()
    {
        world.AIMonitor.OnLocationChanged -= RespondToMovement;
        world.AIMonitor.OnJobSwitched -= RespondToJobSwitch;
        world.AIMonitor.OnDeath -= ReactToJumpscare;
    }

    void RespondToJobSwitch(CharacterName who, JobName fromJob, JobName toJob)
    {
        if (toJob != JobName.WindowWatcher && who == watcherAtThisWindow)
        {
            watcherAtThisWindow = CharacterName.None;
            Image.sprite = noOneAtThisWindow;
        }
    }
    void RespondToMovement(CharacterName who, LocName from, LocName to)
    {
        // Debug.Log($"WindowImage: UpdateWindowImage called for {who} moving from {from} to {to}");

        if (who == watcherAtThisWindow && to != thisWindowDoor)
        {
            watcherAtThisWindow = CharacterName.None;
            Image.sprite = noOneAtThisWindow;
            return;
        }
            
        if (to != thisWindowDoor) return;

        if (!world.TryGetAnimatronic(who, out Animatronic animatronic)) return;
        if (animatronic.GetCurrentJobName() != JobName.WindowWatcher) return;

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

    private bool IsWindowWatcherAtDoor(CharacterName who)
    {
        return world.TryGetAnimatronic(who, out Animatronic animatronic) &&
               animatronic.GetCurrentLocation() == thisWindowDoor &&
               animatronic.GetCurrentJobName() == JobName.WindowWatcher;
    }

}
