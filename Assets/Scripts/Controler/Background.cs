// using UnityEngine;
// using UnityEngine.UI;

// public class Background : MonoBehaviour, IAnimated, IJumpscareAffected
// {
//     private AnimationPlayer animPlayer;
//     public World world;
//     private Image backgroundImage;

//     [Header("Door Animations")]

//     [Header("Right Door")]
//     public AnimationSequence rightDoorOpenAnimation;
//     public AnimationSequence rightDoorCloseAnimation;
//     [Header("Left Door")]
//     public AnimationSequence leftDoorOpenAnimation;
//     public AnimationSequence leftDoorCloseAnimation;

//     [Header("IdleSprites")]

//     [Header("Right Door")]
//     public Sprite rightDoorClosedImage;
//     public Sprite leftDoorOpenImage;

//     [Header("Left Door")]
//     public Sprite leftDoorClosedImage;
//     public Sprite rightDoorOpenImage;

//     void Awake()
//     {
//         backgroundImage = GetComponent<Image>();
//         UtilizeAnimationPlayer();
//     }
  
//     void Start()
//     {
//         backgroundImage.sprite = world.viewDictionary[world.player.currentView].thisViewBackground;
//         world.playerMonitor.OnViewChanged += SwitchBackgroundSprite;
//         world.aiMonitor.OnDeath += ReactToJumpscare;
//     }
//     void OnEnable()
//     {
//         world.playerMonitor.OnActionPerformed += RespondToAction;
//     }

//     void SwitchBackgroundSprite(View newView)
//     {
//         if(newView != View.LeftDoor && newView != View.RightDoor)
//         {
//             backgroundImage.sprite = world.viewDictionary[newView].thisViewBackground;
//         }

//         else if(newView == View.LeftDoor)
//         {
//             if(!world.leftDoorIsOpen)
//             {
//                 backgroundImage.sprite = leftDoorClosedImage;
//             }
//             else
//             {
//                 backgroundImage.sprite = leftDoorOpenImage;
//             }
//         }
//         else if(newView == View.RightDoor)
//         {
//             if(!world.rightDoorIsOpen)
//             {
//                 backgroundImage.sprite = rightDoorClosedImage;
//             }
//             else
//             {
//                 backgroundImage.sprite = rightDoorOpenImage;
//             }
//         }
//     }

//     void RespondToAction(Action action)
//     {
//         if(world.player.currentView != View.LeftDoor && world.player.currentView != View.RightDoor)
//         {
//             return;
//         }
//         if(action == Action.OpenLeftDoor && world.player.currentView == View.LeftDoor && !world.leftDoorIsOpen)
//         {
//             // world.SetLeftDoor(true);
//             animPlayer.PlayAnimation(leftDoorOpenAnimation, gameObject, leftDoorOpenImage);
//         }
//         else if(action == Action.CloseLeftDoor && world.player.currentView == View.LeftDoor && world.leftDoorIsOpen)
//         {
//             // world.SetLeftDoor(false);
//             animPlayer.PlayAnimation(leftDoorCloseAnimation, gameObject, leftDoorClosedImage);
//         }
//         else if(action == Action.OpenRightDoor && world.player.currentView == View.RightDoor && !world.rightDoorIsOpen)
//         {
//             // world.SetRightDoor(true);
//             animPlayer.PlayAnimation(rightDoorOpenAnimation, gameObject, rightDoorOpenImage);
//         }
//         else if(action == Action.CloseRightDoor && world.player.currentView == View.RightDoor && world.rightDoorIsOpen)
//         {
//             // world.SetRightDoor(false);
//             animPlayer.PlayAnimation(rightDoorCloseAnimation, gameObject, rightDoorClosedImage);
//         }
//     }

//     public void UtilizeAnimationPlayer()
//     {
//         animPlayer = GetComponent<AnimationPlayer>();

//         if (animPlayer == null)
//         {
//             animPlayer = gameObject.AddComponent<AnimationPlayer>();
//         }
//     }

//     public void ReactToJumpscare(CharacterName who, LocName from)
//     {
//         if(world.player.currentView != View.LeftDoor && world.player.currentView != View.RightDoor){

//             JumpscareData thsisViewJumpscareData = world.viewDictionary[world.player.currentView].backgroundJumpscareData;

//             if(from == LocName.WestDoor)
//             {
//                 if(who == CharacterName.Bonnie)
//                 {
//                     animPlayer.PlayAnimation(thsisViewJumpscareData.LeftDoorBonnieAnimation, gameObject);
//                 }
//                 else if(who == CharacterName.Chica)
//                 {
//                     animPlayer.PlayAnimation(thsisViewJumpscareData.LeftDoorChicaAnimation, gameObject);
//                 }
//                 else if(who == CharacterName.Foxy)
//                 {
//                     animPlayer.PlayAnimation(thsisViewJumpscareData.LeftDoorFoxyAnimation, gameObject);
//                 }
//                 else if(who == CharacterName.Freddy)
//                 {
//                     animPlayer.PlayAnimation(thsisViewJumpscareData.LeftDoorFreddyAnimation, gameObject);
//                 }
//             }
//             else if(from == LocName.EastDoor)
//             {
//                 if(who == CharacterName.Bonnie)
//                 {
//                     animPlayer.PlayAnimation(thsisViewJumpscareData.RightDoorBonnieAnimation, gameObject);
//                 }
//                 else if(who == CharacterName.Chica)
//                 {
//                     animPlayer.PlayAnimation(thsisViewJumpscareData.RightDoorChicaAnimation, gameObject);
//                 }
//                 else if(who == CharacterName.Foxy)
//                 {
//                     animPlayer.PlayAnimation(thsisViewJumpscareData.RightDoorFoxyAnimation, gameObject);
//                 }
//                 else if(who == CharacterName.Freddy)
//                 {
//                     animPlayer.PlayAnimation(thsisViewJumpscareData.RightDoorFreddyAnimation, gameObject);
//                 }
//             }

//             return;
//         }

//         DoorViewJumpscareData doorViewJumpscareData = world.viewDictionary[world.player.currentView].doorViewJumpscareData;

//         bool thisDoorIsClosed = (!world.rightDoorIsOpen && world.player.currentView == View.RightDoor) || (!world.leftDoorIsOpen && world.player.currentView == View.LeftDoor);

//         bool comingFromOpositeDoor = (from == LocName.WestDoor && world.player.currentView == View.RightDoor) || (from == LocName.EastDoor && world.player.currentView == View.LeftDoor);

//         if (thisDoorIsClosed)
//         {
//             if(who == CharacterName.Bonnie)
//             {
//                 animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorBonnieAnimationDoorClosed, gameObject);
//             }
//             else if(who == CharacterName.Chica)
//             {
//                 animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorChicaAnimationDoorClosed, gameObject);
//             }
//             else if(who == CharacterName.Foxy)
//             {
//                 animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorFoxyAnimationDoorClosed, gameObject);
//             }
//             else if(who == CharacterName.Freddy)
//             {
//                 animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorFreddyAnimationDoorClosed, gameObject);
//             }
//         }
//         else
//         {
//             if (comingFromOpositeDoor)
//             {
//                 if(who == CharacterName.Bonnie)
//                 {
//                     animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorBonnieAnimationDoorOpen, gameObject);
//                 }
//                 else if(who == CharacterName.Chica)
//                 {
//                     animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorChicaAnimationDoorOpen, gameObject);
//                 }
//                 else if(who == CharacterName.Foxy)
//                 {
//                     animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorFoxyAnimationDoorOpen, gameObject);
//                 }
//                 else if(who == CharacterName.Freddy)
//                 {
//                     animPlayer.PlayAnimation(doorViewJumpscareData.OtherDoorFreddyAnimationDoorOpen, gameObject);
//                 }
//             }
//             else
//             {
//                 if(who == CharacterName.Bonnie)
//                 {
//                     animPlayer.PlayAnimation(doorViewJumpscareData.ThisDoorBonnieAnimationDoorOpen, gameObject);
//                 }
//                 else if(who == CharacterName.Chica)
//                 {
//                     animPlayer.PlayAnimation(doorViewJumpscareData.ThisDoorChicaAnimationDoorOpen, gameObject);
//                 }
//                 else if(who == CharacterName.Foxy)
//                 {
//                     animPlayer.PlayAnimation(doorViewJumpscareData.ThisDoorFoxyAnimationDoorOpen, gameObject);
//                 }
//                 else if(who == CharacterName.Freddy)
//                 {
//                     animPlayer.PlayAnimation(doorViewJumpscareData.ThisDoorFreddyAnimationDoorOpen, gameObject);
//                 }
                
//             }
//         }

//         return;
//     }

//     void OnDisable()
//     {
//         world.playerMonitor.OnActionPerformed -= RespondToAction;
//     }
// }
