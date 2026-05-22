using UnityEngine;
using System.Collections.Generic;

public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }
    [field: SerializeField] public Player Player { get; private set; }
    [field: SerializeField] public CameraSystem CameraSystem { get; private set; }
    [field: SerializeField] public AIMonitor AIMonitor { get; private set; }
    [field: SerializeField] public Office OfficeStates { get; private set; }
    public bool IsUnskippableAnimationPlaying = false;

    private readonly Dictionary<CharacterName, Animatronic> animatronics = new();
    private readonly Dictionary<View, ViewController> views = new();

    public IEnumerable<Animatronic> Animatronics => animatronics.Values;
    public IEnumerable<ViewController> ViewControllers => views.Values;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterAnimatronic(CharacterName character, Animatronic animatronic)
    {
        if (character == CharacterName.None || animatronic == null) return;
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

    public void RegisterView(View view, ViewController controller)
    {
        if (view == View.None || controller == null) return;
        views[view] = controller;
    }

    public void UnregisterView(View view, ViewController controller)
    {
        if (!views.TryGetValue(view, out ViewController registered)) return;
        if (registered == controller)
            views.Remove(view);
    }

    public bool TryGetView(View view, out ViewController controller)
    {
        return views.TryGetValue(view, out controller);
    }
}
