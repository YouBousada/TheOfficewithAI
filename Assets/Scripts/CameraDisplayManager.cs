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
        public RoomName Room;
        public BehaviourName AnimBehaviour;
        public int SortingLayer;
    }

    private struct AnimatronicState
    {
        public RoomName Location;
        public BehaviourName Behaviour;
    }

    [Header("Dependencies")]
    [SerializeField] private TotalCameraData totalCameraData;
    [SerializeField] private CameraSystemConfig cameraSystemConfig;

    [Header("Camera View Layers")]
    [SerializeField] private List<Image> characterLayers;
    [SerializeField] private Image roomRenderImage;

    [Header("Empty Sprite")]
    [SerializeField] private Sprite emptySprite;

     [Header("Cachable Behaviours")]
    [SerializeField] private List<BehaviourName> CachableBehaviours;

    private CamDisplayLayer[] camDisplay;
    private Dictionary<CharacterName, CachedRender> renderCache = new Dictionary<CharacterName, CachedRender>();
    private Dictionary<CharacterName, AnimatronicState> animatronicStates = new Dictionary<CharacterName, AnimatronicState>();

    private RoomCameraData thisRoomCameraData;
    private RoomName activeCamera;

    private void Awake()
    {
        if (totalCameraData == null)      Debug.LogWarning($"{name}: totalCameraData is not assigned.", this);
        if (roomRenderImage == null)      Debug.LogWarning($"{name}: backgroundLayer is not assigned.", this);
        if (emptySprite == null)          Debug.LogWarning($"{name}: emptySprite is not assigned.", this);
        if (characterLayers == null || characterLayers.Count == 0)
            Debug.LogWarning($"{name}: characterLayers is not assigned.", this);

        camDisplay = new CamDisplayLayer[characterLayers.Count];
    }

    private void OnEnable()
    {
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
        HandleCameraRoomChanged(cameraSystemConfig.startingRoom);
    }

    public void HandleAnimatronicInitialization(InitializationData data)
    {
        animatronicStates[data.who] = new AnimatronicState { Location = data.location, Behaviour = data.behaviour };
    }

    public void HandleBehaviourChanged(BehaviourChangedData data)
    {
        CharacterName who = data.who;
        BehaviourName newBehaviour = data.newBehaviour;
        RoomName location = data.location;

        animatronicStates[who] = new AnimatronicState { Location = location, Behaviour = newBehaviour };

        if (location != activeCamera) return;

        renderCache.Remove(who);
        RemoveCharacterFromDisplay(who);
        AddCharacterToDisplay(who, newBehaviour);
    }

    public void HandleAnimatronicMovement(LocationChangedData data)
    {
        CharacterName who = data.who;
        RoomName from = data.from;
        RoomName to = data.to;
        BehaviourName behaviour = data.behaviour;

        animatronicStates[who] = new AnimatronicState { Location = to, Behaviour = behaviour };
        renderCache.Remove(who);

        if (to == activeCamera)
            AddCharacterToDisplay(who, behaviour);
        else if (from == activeCamera)
            RemoveCharacterFromDisplay(who);
    }

    public void HandleCameraRoomChanged(RoomName newRoom)
    {
        ClearDisplay();

        activeCamera = newRoom;

        thisRoomCameraData = totalCameraData.GetRoomCameraData(newRoom);

        if (thisRoomCameraData == null)
        {
            return;
        }

        roomRenderImage.sprite = thisRoomCameraData.backgroundSprite;

        AddCharactersToDisplay();
    }

    private void AddCharactersToDisplay()
    {
        foreach (var kvp in animatronicStates)
        {
            if (kvp.Value.Location == activeCamera)
                AddCharacterToDisplay(kvp.Key, kvp.Value.Behaviour);
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
            return;
        }

        //choose from avilable layers at random, unless stage, then just use stage allocated layer
        int chosenLayer;

        if (activeCamera == RoomName.STAGE)
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
