using UnityEngine;
using System.Collections.Generic;
using System;

public class World : MonoBehaviour
{
    public Player player;
    public bool leftDoorIsOpen;
    public bool rightDoorIsOpen;

    public event Action<bool, bool> OnDoorStateChanged;

    public void SetLeftDoor(bool isOpen)
    {
        if (leftDoorIsOpen == isOpen) return;
        leftDoorIsOpen = isOpen;
        OnDoorStateChanged?.Invoke(leftDoorIsOpen, rightDoorIsOpen);
    }

    public void SetRightDoor(bool isOpen)
    {
        if (rightDoorIsOpen == isOpen) return;
        rightDoorIsOpen = isOpen;
        OnDoorStateChanged?.Invoke(leftDoorIsOpen, rightDoorIsOpen);
    }

    [field: SerializeField] public CameraSystem cameraSystem { get; private set; }
    [field: SerializeField] public AIMonitor aiMonitor { get; private set; }
    [field: SerializeField] public GameManager gameManager { get; private set; }
    [field: SerializeField] public List<Animatronic> animatronicList = new List<Animatronic>();
    public Dictionary<CharacterName, Animatronic> animatronicSet = new Dictionary<CharacterName, Animatronic>();
    [field: SerializeField] public List<ViewController> gameViews;
    public Dictionary<View, ViewController> viewDictionary = new Dictionary<View, ViewController>();
    public void Awake()
    {
        for (int i = 0; i < animatronicList.Count; i++)
        {
            animatronicSet.Add(animatronicList[i].GetCharacterName(), animatronicList[i]);
        }

        for (int i = 0; i < gameViews.Count; i++)
        {
            viewDictionary.Add(gameViews[i].thisViewName, gameViews[i]);
        }

        leftDoorIsOpen = true;
        rightDoorIsOpen = true;
    }

    public void Start()
    {
        player.OnActionPerformed += HandleAction;
    }

    void HandleAction(Action action)
    {
        if (action == Action.OpenLeftDoor) SetLeftDoor(true);
        else if (action == Action.CloseLeftDoor) SetLeftDoor(false);
        else if (action == Action.OpenRightDoor) SetRightDoor(true);
        else if (action == Action.CloseRightDoor) SetRightDoor(false);
    }

    void OnDestroy()
    {
        player.OnActionPerformed -= HandleAction;
    }
}
