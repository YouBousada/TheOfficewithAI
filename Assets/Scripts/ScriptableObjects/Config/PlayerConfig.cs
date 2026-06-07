using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private ViewName StartingView = ViewName.DESK;
    [SerializeField] private Item StartingItemInHand;
    public ViewName startingview => StartingView;
    public Item startingItemInHand => StartingItemInHand;
}
