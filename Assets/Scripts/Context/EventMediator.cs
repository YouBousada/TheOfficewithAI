using UnityEngine;

public class EventMediator : MonoBehaviour
{
    [field: SerializeField] private Player player;
    [field: SerializeField] private PuzzleMaster puzzleMaster;

    void Start()
    {
        player.OnReturnItem += puzzleMaster.giveItem;
    }

    void OnDestroy()
    {
        player.OnReturnItem -= puzzleMaster.giveItem;
    }
}
