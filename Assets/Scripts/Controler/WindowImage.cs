using UnityEngine;
using UnityEngine.UI;

public class WindowImage : MonoBehaviour, IAnimated, IJumpscareAffected
{
    public World world;
    public LocName thisWindowDoor;
    private Image Image;
    private AnimationPlayer animPlayer;
    private CharacterName WatcherAtThisWindow = CharacterName.None;

    [Header("Idle Window Sprites")]
    public Sprite noOneAtThisWindow;
    public Sprite bonnieAtThisWindow;
    public Sprite chicaAtThisWindow;
    public Sprite foxyAtThisWindow;
    public Sprite freddyAtThisWindow;   

    [Header("JumpscareData")]
    public WindowJumpscareData jumpscareData;

    void Awake()
    {
        UtilizeAnimationPlayer();
        Image = GetComponent<Image>();
    }

    void OnEnable()
    {
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
        if(who == WatcherAtThisWindow)
        {
            Image.sprite = noOneAtThisWindow;
            return;
        }
        
        if((from == LocName.EastDoor && thisWindowDoor == LocName.EastDoor) || (from == LocName.WestDoor && thisWindowDoor == LocName.WestDoor))
        {
            if(WatcherAtThisWindow == CharacterName.Bonnie)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereBonnieAnimation, gameObject);
            }
            else if(WatcherAtThisWindow == CharacterName.Chica)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereChicaAnimation, gameObject);
            }
            else if(WatcherAtThisWindow == CharacterName.Foxy)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereFoxyAnimation, gameObject);
            }
            else if(WatcherAtThisWindow == CharacterName.Freddy)
            {
                animPlayer.PlayAnimation(jumpscareData.FromHereFreddyAnimation, gameObject);
            }
            return;
        }

        if((from == LocName.EastDoor && thisWindowDoor == LocName.WestDoor) || (from == LocName.WestDoor && thisWindowDoor == LocName.EastDoor))
        {
            if(WatcherAtThisWindow == CharacterName.Bonnie)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorBonnieAnimation, gameObject);
            }
            else if(WatcherAtThisWindow == CharacterName.Chica)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorChicaAnimation, gameObject);
            }
            else if(WatcherAtThisWindow == CharacterName.Foxy)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorFoxyAnimation, gameObject);
            }
            else if(WatcherAtThisWindow == CharacterName.Freddy)
            {
                animPlayer.PlayAnimation(jumpscareData.FromOtherDoorFreddyAnimation, gameObject);
            }
            return;
        }
        
    }

    private void DoSubscribing()
    {
        world.aiMonitor.OnLocationChanged += RespondToMovement;
        world.aiMonitor.OnJobSwitched += RespondToJobSwitch;
        world.aiMonitor.OnDeath += ReactToJumpscare;
    }

    private void SeeIfAWindowWatcherIsHere()
    {
        if (world.animatronicSet[CharacterName.Bonnie].GetCurrentLocation() == thisWindowDoor && world.animatronicSet[CharacterName.Bonnie].GetCurrentJobName() == JobName.WindowWatcher)
        {
            RespondToMovement(CharacterName.Bonnie, LocName.None, thisWindowDoor);
            // Debug.Log("WindowImage: Bonnie found at his window on enable");
        }
        else if (world.animatronicSet[CharacterName.Chica].GetCurrentLocation() == thisWindowDoor && world.animatronicSet[CharacterName.Chica].GetCurrentJobName() == JobName.WindowWatcher)
        {
            RespondToMovement(CharacterName.Chica, LocName.None, thisWindowDoor);
            // Debug.Log("WindowImage: Chica found at her window on enable");
        }
        else if (world.animatronicSet[CharacterName.Foxy].GetCurrentLocation() == thisWindowDoor && world.animatronicSet[CharacterName.Foxy].GetCurrentJobName() == JobName.WindowWatcher)
        {
            RespondToMovement(CharacterName.Foxy, LocName.None, thisWindowDoor);
            // Debug.Log("WindowImage: Foxy found at his window on enable");
        }
        else if (world.animatronicSet[CharacterName.Freddy].GetCurrentLocation() == thisWindowDoor && world.animatronicSet[CharacterName.Freddy].GetCurrentJobName() == JobName.WindowWatcher)
        {
            RespondToMovement(CharacterName.Freddy, LocName.None, thisWindowDoor);
            // Debug.Log("WindowImage: Freddy found at his window on enable");
        }
        else
        {
            WatcherAtThisWindow = CharacterName.None;
            Image.sprite = noOneAtThisWindow;
            // Debug.Log("WindowImage: No one found at this window on enable");
        }
    }

    void OnDisable()
    {
        world.aiMonitor.OnLocationChanged -= RespondToMovement;
        world.aiMonitor.OnJobSwitched -= RespondToJobSwitch;
        world.aiMonitor.OnDeath -= ReactToJumpscare;
    }

    void RespondToJobSwitch(CharacterName who, JobName fromJob, JobName toJob)
    {
        if (toJob != JobName.WindowWatcher && who == WatcherAtThisWindow)
        {
            WatcherAtThisWindow = CharacterName.None;
            Image.sprite = noOneAtThisWindow;
        }
    }
    void RespondToMovement(CharacterName who, LocName from, LocName to)
    {
        Debug.Log($"WindowImage: UpdateWindowImage called for {who} moving from {from} to {to}");

        if (who == WatcherAtThisWindow && to != thisWindowDoor)
        {
            WatcherAtThisWindow = CharacterName.None;
            Image.sprite = noOneAtThisWindow;
            return;
        }
            
        if (to != thisWindowDoor) return;

        if (world.animatronicSet[who].GetCurrentJobName() != JobName.WindowWatcher) return;

        WatcherAtThisWindow = who;

        Image.sprite = who switch
        {
            CharacterName.Bonnie => bonnieAtThisWindow,
            CharacterName.Chica  => chicaAtThisWindow,
            CharacterName.Foxy   => foxyAtThisWindow,
            CharacterName.Freddy => freddyAtThisWindow,
            _ => noOneAtThisWindow
        };
    }

}
