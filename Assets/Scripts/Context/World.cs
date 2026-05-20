using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public static World Instance { get; private set; }

    [field: SerializeField] public Player Player { get; private set; }
    [field: SerializeField] public CameraSystem CameraSystem { get; private set; }
    [field: SerializeField] public AIMonitor AIMonitor { get; private set; }
    [field: SerializeField] public Office OfficeStates { get; private set; }

    public Dictionary<CharacterName, Animatronic> animatronicDictionary = new Dictionary<CharacterName, Animatronic>();
    public Dictionary<View, ViewController> viewDictionary = new Dictionary<View, ViewController>();

    void Awake()
    {
        Instance = this;
    }
}
