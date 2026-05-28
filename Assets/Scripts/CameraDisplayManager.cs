using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraDisplayManager : MonoBehaviour
{
    private struct CamDisplayLayer
    {
        public Image LayerImage;
        public CharacterName Occupant;
    }

    private struct CachedRender
    {
        public Sprite RenderSprite;
        public LocName Room;
        public BehaviourName AnimBehaviour;
        public int SortingLayer;
    }

    [Header("Dependencies")]
    [SerializeField] private CameraSystem cameraSystem;
    [SerializeField] private AIEvents aiMonitor;
    [SerializeField] private AnimatronicRegistry animatronicRegistry;
    [SerializeField] private TotalCameraData totalCameraData;

    [Header("Camera View Layers")]
    [SerializeField] private List<Image> characterLayers;
    [SerializeField] private Image backgroundLayer;

    [Header("Empty Sprite")]
    [SerializeField] private Sprite emptySprite;

     [Header("Cachable Behaviours")]
    [SerializeField] private List<BehaviourName> CachableBehaviours;

    private CamDisplayLayer[] camDisplay;
    private Dictionary<CharacterName, CachedRender> renderCache = new Dictionary<CharacterName, CachedRender>();

    private RoomCameraData thisRoomCameraData;
    private LocName activeCamera;

    private void Awake()
    {
        if (cameraSystem == null)         Debug.LogWarning($"{name}: cameraSystem is not assigned.", this);
        if (aiMonitor == null)            Debug.LogWarning($"{name}: aiMonitor is not assigned.", this);
        if (animatronicRegistry == null)  Debug.LogWarning($"{name}: animatronicRegistry is not assigned.", this);
        if (totalCameraData == null)      Debug.LogWarning($"{name}: totalCameraData is not assigned.", this);
        if (backgroundLayer == null)      Debug.LogWarning($"{name}: backgroundLayer is not assigned.", this);
        if (emptySprite == null)          Debug.LogWarning($"{name}: emptySprite is not assigned.", this);
        if (characterLayers == null || characterLayers.Count == 0)
            Debug.LogWarning($"{name}: characterLayers is not assigned.", this);

        camDisplay = new CamDisplayLayer[characterLayers.Count];
    }

    private void OnEnable()
    {
        activeCamera = cameraSystem.ActiveCamera;

        aiMonitor.OnLocationChanged += HandleAnimatronicMovement;
        aiMonitor.OnBehaviourChanged += HandleBehaviourChanged;
        cameraSystem.OnCameraRoomChanged += HandleCameraRoomChanged;

        for (int i = 0; i < characterLayers.Count; i++)
        {
            camDisplay[i] = new CamDisplayLayer
            {
                LayerImage = characterLayers[i],
                Occupant = CharacterName.NONE
            };

            camDisplay[i].LayerImage.sprite = emptySprite;
        }
    }

    private void Start()
    {
        HandleCameraRoomChanged(cameraSystem.ActiveCamera);
    }

    private void OnDisable()
    {
        aiMonitor.OnLocationChanged -= HandleAnimatronicMovement;
        aiMonitor.OnBehaviourChanged -= HandleBehaviourChanged;
        cameraSystem.OnCameraRoomChanged -= HandleCameraRoomChanged;
    }

    private void HandleBehaviourChanged(CharacterName who, BehaviourName oldBehaviour, BehaviourName newBehaviour)
    {
        if (!animatronicRegistry.TryGetAnimatronic(who, out Animatronic animatronic)) return;
        if (animatronic.GetCurrentLocation() != activeCamera) return;

        renderCache.Remove(who);
        RemoveCharacterFromDisplay(who);
        AddCharacterToDisplay(who, newBehaviour);
    }

    private void HandleAnimatronicMovement(CharacterName who, LocName from, LocName to)
    {
        renderCache.Remove(who);

        if (!animatronicRegistry.TryGetAnimatronic(who, out Animatronic animatronic)) return;
        BehaviourName behaviour = animatronic.GetCurrentBehaviour();

        if (to == activeCamera)
            AddCharacterToDisplay(who, behaviour);
        else if (from == activeCamera)
            RemoveCharacterFromDisplay(who);
    }

    private void HandleCameraRoomChanged(LocName newRoom)
    {
        ClearDisplay();

        activeCamera = newRoom;

        thisRoomCameraData = totalCameraData.GetRoomCameraData(newRoom);

        if (thisRoomCameraData == null)
        {
            // Debug.LogWarning($"{name}: Could not find RoomCameraData for location: {newRoom}");
            return;
        }

        backgroundLayer.sprite = thisRoomCameraData.backgroundSprite;

        AddCharactersToDisplay();
    }

    private void AddCharactersToDisplay()
    {
        foreach (Animatronic anim in animatronicRegistry.Animatronics)
        {
            if (anim.GetCurrentLocation() == activeCamera)
                AddCharacterToDisplay(anim.GetCharacterName(), anim.GetCurrentBehaviour());
        }
    }

    private void AddCharacterToDisplay(CharacterName animName, BehaviourName animBehaviour)
    {
        if (thisRoomCameraData == null) return;

        //check caache, use cache render if cache hit and return
        bool cachable = CachableBehaviours.Contains(animBehaviour);

        if (cachable && renderCache.TryGetValue(animName, out var cached) && cached.Room == activeCamera && cached.AnimBehaviour == animBehaviour)
        {
            PasteInRender(cached.SortingLayer, cached.RenderSprite, animName);
            return;
        }

        //check for available layers
        List<int> availableLayers = new List<int>();
        for (int i = 0; i < camDisplay.Length; i++)
        {
            if (camDisplay[i].Occupant == CharacterName.NONE)
                availableLayers.Add(i);
        }

        if (availableLayers.Count == 0)
        {
            // Debug.LogWarning($"{name}: No available layers to display {animName}.");
            return;
        }

        //choose from avilable layers at random, unless stage, then just use stage allocated layer
        int chosenLayer;

        if (activeCamera == LocName.STAGE)
        {
            chosenLayer = totalCameraData.GetStageLayerForCharacter(animName);
        }
        else
        {
            chosenLayer = availableLayers[Random.Range(0, availableLayers.Count)];
        }

        //add render to display
        var camviewData = thisRoomCameraData.GetCamviewDataForCharacter(animName);
        if (camviewData == null) return;
        Sprite chosenRender = camviewData.GetRandomRender(chosenLayer, animBehaviour);
        PasteInRender(chosenLayer, chosenRender, animName);

        //cache render if idling or running
        if (cachable)
        {
            renderCache[animName] = new CachedRender
            {
                RenderSprite = chosenRender,
                Room = activeCamera,
                AnimBehaviour = animBehaviour,
                SortingLayer = chosenLayer
            };
        }
    }

    private void RemoveCharacterFromDisplay(CharacterName who)
    {
        for (int i = 0; i < camDisplay.Length; i++)
        {
            if (camDisplay[i].Occupant == who)
            {
                camDisplay[i].LayerImage.sprite = emptySprite;
                camDisplay[i].Occupant = CharacterName.NONE;
                return;
            }
        }
    }

    private void PasteInRender(int sortingLayerIndex, Sprite render, CharacterName occupant)
    {
        camDisplay[sortingLayerIndex].LayerImage.sprite = render;
        camDisplay[sortingLayerIndex].Occupant = occupant;
    }

    private void ClearDisplay()
    {
        for (int i = 0; i < camDisplay.Length; i++)
        {
            camDisplay[i].Occupant = CharacterName.NONE;
            camDisplay[i].LayerImage.sprite = emptySprite;
        }
    }
}
