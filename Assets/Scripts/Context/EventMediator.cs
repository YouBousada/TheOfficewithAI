using UnityEngine;

public class EventMediator : MonoBehaviour
{
    [field: SerializeField] public Player Player { get; private set; }
    [field: SerializeField] public PuzzleMaster puzzleMaster { get; private set; }

    void Start()
    {
        Player.OnReturnItem += puzzleMaster.giveItem;
    }

    void OnDestroy()
    {
        Player.OnReturnItem -= puzzleMaster.giveItem;
    }
}
