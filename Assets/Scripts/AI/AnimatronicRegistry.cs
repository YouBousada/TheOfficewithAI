using System.Collections.Generic;
using UnityEngine;

public class AnimatronicRegistry : MonoBehaviour
{
    private readonly Dictionary<CharacterName, Animatronic> animatronics = new();
    public IEnumerable<Animatronic> Animatronics => animatronics.Values;

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

    void OnGUI()
    {
        GUIStyle style = new(GUI.skin.label) { fontSize = 20 };
        style.normal.textColor = Color.green;

        int i = 0;
        foreach (Animatronic anim in animatronics.Values)
        {
            GUI.Label(
                new Rect(300, 10 + i * 30, 500, 40),
                $"{anim.GetCharacterName()}'s Job: {anim.GetCurrentJobName()}",
                style
            );
            i++;
        }
    }
}
