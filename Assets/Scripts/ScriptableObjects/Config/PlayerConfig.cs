using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Config/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [SerializeField] private ViewName StartingView = ViewName.DESK;
    public ViewName startingview => StartingView;
}
