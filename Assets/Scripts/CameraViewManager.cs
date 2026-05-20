using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraViewManager : MonoBehaviour
{
    [Header("World and Camera Data")]
    [SerializeField] private World world;
    [SerializeField] private TotalCameraData totalCameraData;
    

    [Header("Camera View Layers")]
    [SerializeField] private Image layer1;
    [SerializeField] private Image layer2;
    [SerializeField] private Image layer3;
    [SerializeField] private Image layer4;
    [SerializeField] private Image backgroundLayer;

    [Header("Empty Sprite")]
    [SerializeField] private Sprite emptySprite;

    private Dictionary<CharacterName, CharacterCamviewData> characterRendersMap = new Dictionary<CharacterName, CharacterCamviewData>();
    private Dictionary<Image, CharacterName> imageToCharacter = new Dictionary<Image, CharacterName>();
    private Dictionary<CharacterName, Image> characterToLayer = new Dictionary<CharacterName, Image>();
    private Dictionary<int, Image> sortingLayerToImage = new Dictionary<int, Image>();

    private RoomCameraData thisRoomCameraData;
    private LocName ActiveCamera;

    

    public void Awake()
    {
        BuildSortingLayerMap();

        ClearView();
    }

    private void ClearView()
    {
        ClearLayerSprites();
        ClearLayerToCharacterMap();
        ClearCharacterToLayerMap();
    }

    public void OnEnable()
    {
        ActiveCamera = world.CameraSystem.ActiveCamera;

        world.AIMonitor.OnLocationChanged += HandleAnimatronicMovement;
        world.AIMonitor.OnBehaviourChanged += HandleAnimatronicBehaviorChange;
        world.CameraSystem.OnCameraRoomChanged += HandleCameraRoomChanged;
    }

    public void Start()
    {
        HandleCameraRoomChanged(world.CameraSystem.ActiveCamera);
    }


    private void HandleCameraRoomChanged(LocName newRoom)
    {
        ActiveCamera = newRoom;

        ClearView();

        thisRoomCameraData = totalCameraData.GetRoomCameraData(newRoom);
        backgroundLayer.sprite = thisRoomCameraData.backgroundSprite;

        if(thisRoomCameraData == null)
        {
            Debug.LogWarning("CameraView " + gameObject.name + " could not find RoomCameraData for location: " + newRoom);
            return;
        }

        BuildCharacterRenderMap();

        AddCharactersToView();
    }

    private void AddCharactersToView()
    {
        foreach (Animatronic anim in world.animatronicDictionary.Values)
        {
            if (anim.GetCurrentLocation() == ActiveCamera)
            {
                AddCharacter(anim.GetCharacterName(), anim.GetCurrentBehaviour());
            }
        }
    }

    public void OnDisable()
    {
        world.AIMonitor.OnLocationChanged -= HandleAnimatronicMovement;
        world.AIMonitor.OnBehaviourChanged -= HandleAnimatronicBehaviorChange;
        world.CameraSystem.OnCameraRoomChanged -= HandleCameraRoomChanged;
    }

    public void HandleAnimatronicMovement(CharacterName who, LocName from, LocName to)
    {
        if (to == ActiveCamera)
        {
            AddCharacter(who, world.animatronicDictionary[who].GetCurrentBehaviour());
        }
        else if (from == ActiveCamera)
        {
            RemoveCharacterFromThisView(who);
        }   
    }

    public void HandleAnimatronicBehaviorChange(CharacterName who, Behaviour oldBehaviour, Behaviour newBehaviour)
    {
        if(world.animatronicDictionary[who].GetCurrentLocation() != ActiveCamera)
        {
            return;
        }
        else
        {
            RemoveCharacterFromThisView(who);
            AddCharacter(who , newBehaviour);
        }
    }


    public void AddCharacter(CharacterName thisCharacter, Behaviour behaviour)
    {
        if (characterToLayer.TryGetValue(thisCharacter, out var existingLayer) && existingLayer != null)
        {
            Debug.LogWarning("Camera view " + gameObject.name + " tried to add " + thisCharacter + " but they are already in this camera view.");
            return;
        }

        if (NoOpenLayers())
        {
            Debug.LogWarning("Camera view " + gameObject.name + " tried to add a character when no available layers to add character to camera view.");
            return;
        }

        List<int> availableLayers = GetAvailableSortingLayers();

        CharacterCamviewData thisCharacterRenders = characterRendersMap[thisCharacter];

        //CHECK CACHE HERE, GET LAYER AND IMAGE

        int layerInt = -1;

        if(ActiveCamera  == LocName.Stage)
        {
           layerInt = totalCameraData.GetStageLayerForCharacter(thisCharacter);
        }
        else
        {
            //CHOOSES THE LAYER
            layerInt = availableLayers[Random.Range(0, availableLayers.Count)];
        }
        //CHOOSES THE RENDER 
        Sprite chosenRender = thisCharacterRenders.GetRandomRender(layerInt, behaviour);

        if (chosenRender == null)
        {
            Debug.LogWarning("Camera view " + gameObject.name + " could not get a valid CharacterCameraRender for " + thisCharacter + " at layer " + layerInt + " with behaviour " + behaviour + "check data.");
            return;
        }

        PasteInCharacter(thisCharacter, chosenRender, sortingLayerToImage[layerInt]);

    }

    private void PasteInCharacter(CharacterName characterName, Sprite render, Image thisLayer)
    {
        imageToCharacter[thisLayer] = characterName;
        characterToLayer[characterName] = thisLayer;
        thisLayer.sprite = render;
    }

    private List<int> GetAvailableSortingLayers()
    {
        //CHECK CACHE, TAKEN LAYERS WILL NOT BE AVIALABLE

        List<int> availableLayers = new List<int>();

        foreach (var entry in sortingLayerToImage)
        {
            Image image = entry.Value;

            if (imageToCharacter[image] == CharacterName.None)
            {
                availableLayers.Add(entry.Key);
            }
        }

        return availableLayers;
    }

    private bool NoOpenLayers()
    {
        return imageToCharacter[layer1] != CharacterName.None &&
            imageToCharacter[layer2] != CharacterName.None &&
            imageToCharacter[layer3] != CharacterName.None &&
            imageToCharacter[layer4] != CharacterName.None;
    }

    public void RemoveCharacterFromThisView(CharacterName characterName)
    {
        if (!characterToLayer.TryGetValue(characterName, out var img) || img == null)
        {
            Debug.LogWarning($"Camera view {name} tried to remove {characterName} but it isn't here.");
            return;
        }

        img.sprite = emptySprite;
        imageToCharacter[img] = CharacterName.None;
        characterToLayer[characterName] = null;
    }

    public void ClearCharacterToLayerMap()
    {
        characterToLayer[CharacterName.Bonnie] = null;
        characterToLayer[CharacterName.Chica] = null;
        characterToLayer[CharacterName.Foxy] = null;
        characterToLayer[CharacterName.Freddy] = null;
    }

    private void ClearLayerToCharacterMap()
    {
        imageToCharacter[layer1] = CharacterName.None;
        imageToCharacter[layer2] = CharacterName.None;
        imageToCharacter[layer3] = CharacterName.None;
        imageToCharacter[layer4] = CharacterName.None;
    }

    private void BuildCharacterRenderMap()
    {
        characterRendersMap[CharacterName.Bonnie] = thisRoomCameraData.GetCamviewDataForCharacter(CharacterName.Bonnie);
        characterRendersMap[CharacterName.Chica] = thisRoomCameraData.GetCamviewDataForCharacter(CharacterName.Chica);
        characterRendersMap[CharacterName.Foxy] = thisRoomCameraData.GetCamviewDataForCharacter(CharacterName.Foxy);
        characterRendersMap[CharacterName.Freddy] = thisRoomCameraData.GetCamviewDataForCharacter(CharacterName.Freddy);
    }

    private void BuildSortingLayerMap()
    {
        sortingLayerToImage[1] = layer1;
        sortingLayerToImage[2] = layer2;
        sortingLayerToImage[3] = layer3;
        sortingLayerToImage[4] = layer4;
    }

    private void ClearLayerSprites()
    {
        layer1.sprite = emptySprite;
        layer2.sprite = emptySprite;
        layer3.sprite = emptySprite;
        layer4.sprite = emptySprite;
    }
}

//IDK