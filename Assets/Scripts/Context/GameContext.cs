using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }
    [field: SerializeField] public Player Player { get; private set; }
    [field: SerializeField] public CameraSystem CameraSystem { get; private set; }
    [field: SerializeField] public AIEvents AIMonitor { get; private set; }
    [field: SerializeField] public Office OfficeStates { get; private set; }
    [field: SerializeField] public PuzzleMaster puzzleMaster { get; private set; }

    private readonly Dictionary<CharacterName, Animatronic> animatronics = new();
    public IEnumerable<Animatronic> Animatronics => animatronics.Values;

    void Awake()
    {
        Instance = this;
    }

    void OnGUI()
    {
        GUIStyle style = new(GUI.skin.label) { fontSize = 20 };
        style.normal.textColor = Color.green;

        int i = 0;

        foreach (Animatronic animatronic in Animatronics)
        {
            GUI.Label(
                new Rect(300, 10 + i * 30, 500, 40),
                $"{animatronic.GetCharacterName()}'s Job: {animatronic.GetCurrentJobName()}",
                style
            );

            i++;
        }
    }

    public void RegisterAnimatronic(CharacterName character, Animatronic animatronic)
    {
        if (character == CharacterName.NONE || animatronic == null) return;
        animatronics[character] = animatronic;
    }

    public void UnregisterAnimatronic(CharacterName character, Animatronic animatronic)
    {
        if (!animatronics.TryGetValue(character, out Animatronic registered)) return;
        if (registered == animatronic)
            animatronics.Remove(character);
    }

    public bool TryGetAnimatronic(CharacterName character, out Animatronic animatronic)
    {
        return animatronics.TryGetValue(character, out animatronic);
    }
}
