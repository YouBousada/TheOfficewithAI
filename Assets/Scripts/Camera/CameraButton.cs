using UnityEngine;
[RequireComponent(typeof(UnityEngine.UI.Button))]

public class CameraButton : HotSpot
{
    [Header("CameraButton requirements")]
    [SerializeField] private RoomName thisButtonsRoom;
    [SerializeField] private CameraChangedEvent cameraChangedEvent;
    [SerializeField] private ActiveCameraVariable activeCamera;


    protected override void Awake()
    {
        base.Awake();

        AddActiveViewIfMissing(ViewName.MONITOR);

        button.onClick.AddListener(OnButtonPressed);
    }

    void Reset()
    {
        AddActiveViewIfMissing(ViewName.MONITOR);
    }

    public void OnButtonPressed()
    {
        if (!IsInActiveView) return;

        activeCamera.room = thisButtonsRoom;
        cameraChangedEvent.Raise(thisButtonsRoom);
    }

}
