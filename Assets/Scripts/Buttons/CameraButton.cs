using UnityEngine;

public class CameraButton : OverLayObject
{
    [SerializeField] private LocName thisRoom;
    [SerializeField] private CameraSystem cameraSystem;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        AddActiveViewIfMissing(ViewName.MONITOR);

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    protected override void Start()
    {
        base.Start();

        if (cameraSystem == null && gameContext != null)
            cameraSystem = gameContext.CameraSystem;
    }

    void Reset()
    {
        AddActiveViewIfMissing(ViewName.MONITOR);
    }

    public void OnButtonPressed()
    {
        if (!IsInActiveView) return;
        if (cameraSystem == null)
        {
            Debug.LogWarning($"{name} cannot switch cameras because no CameraSystem is assigned.", this);
            return;
        }

        // Debug.Log("Camera button for " + thisRoom + " pressed.");
        cameraSystem.SetCameraRoom(thisRoom);
    }

    protected override void EnterActiveView()
    {
        SetVisible(true);
    }

    protected override void ExitActiveView()
    {
        SetVisible(false);
    }

    protected override void HideForJumpscare(CharacterName who, LocName from)
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
