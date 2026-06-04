using UnityEngine;

public class EventMediator : MonoBehaviour
{
    [field: SerializeField] private Player player;
    [field: SerializeField] private PuzzleMaster puzzleMaster;


    void OnEnable()
    {
        player.OnReturnItem += puzzleMaster.giveItem;
    }

    void OnDestroy()
    {
        player.OnReturnItem -= puzzleMaster.giveItem;

    }
}
