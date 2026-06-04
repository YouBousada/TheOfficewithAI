using UnityEngine;
[RequireComponent(typeof(UnityEngine.UI.Button))]

public class CameraButton : HotSpot
{
    [Header("CameraButton requirements")]
    [SerializeField] private RoomName thisRoom;

    [SerializeField] private CameraChangedEvent cameraChangedEvent;


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

        cameraChangedEvent.Raise(thisRoom);
    }

    protected override void EnterActiveView()
    {
        SetVisible(true);
    }

    protected override void ExitActiveView()
    {
        SetVisible(false);
    }

    protected override void HideForJumpscare(CharacterName who, RoomName from)
    {
        SetVisible(false);
    }

    private void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}
