using UnityEngine;

public class EventMediator : MonoBehaviour
{
    [field: SerializeField] private Player player;
    [field: SerializeField] private PuzzleMaster puzzleMaster;
    [field: SerializeField] private AiMonitor aIEvents;
    [field: SerializeField] private CameraDisplayManager cameraDisplayManager;
    [field: SerializeField] private CameraSystem cameraSystem;

    void OnEnable()
    {
        player.OnReturnItem += puzzleMaster.giveItem;
        aIEvents.OnInitialization += cameraDisplayManager.HandleAnimatronicInitialization;
        aIEvents.OnLocationChanged += cameraDisplayManager.HandleAnimatronicMovement;
        aIEvents.OnBehaviourChanged += cameraDisplayManager.HandleBehaviourChanged;
        cameraSystem.OnCameraRoomChanged += cameraDisplayManager.HandleCameraRoomChanged;
    }

    void OnDestroy()
    {
        player.OnReturnItem -= puzzleMaster.giveItem;
        aIEvents.OnInitialization -= cameraDisplayManager.HandleAnimatronicInitialization;
        aIEvents.OnLocationChanged -= cameraDisplayManager.HandleAnimatronicMovement;
        aIEvents.OnBehaviourChanged -= cameraDisplayManager.HandleBehaviourChanged;
        cameraSystem.OnCameraRoomChanged -= cameraDisplayManager.HandleCameraRoomChanged;
    }
}
