using System.Collections.Generic;

public class AnimatronicRegistry
{
    private static AnimatronicRegistry _instance;
    public static AnimatronicRegistry Instance => _instance ??= new AnimatronicRegistry();

    public readonly Dictionary<CharacterName, Animatronic> animatronicDict = new();
    public IEnumerable<Animatronic> animatronicList => animatronicDict.Values;

    public void RegisterAnimatronic(CharacterName character, Animatronic animatronic)
    {
        if (character == CharacterName.NONE || animatronic == null) return;
        animatronicDict[character] = animatronic;
    }

    public void UnregisterAnimatronic(CharacterName character, Animatronic animatronic)
    {
        if (!animatronicDict.TryGetValue(character, out Animatronic registered)) return;
        if (registered == animatronic)
            animatronicDict.Remove(character);
    }

    public bool TryGetAnimatronic(CharacterName character, out Animatronic animatronic)
    {
        return animatronicDict.TryGetValue(character, out animatronic);
    }

    public int AnimatronicsInThisRoom(RoomName room)
    {
        int animsInThisRoom = 0;

        foreach(Animatronic a in animatronicList)
        {
            if(a.GetCurrentLocation() == room)
            {
                animsInThisRoom++;
            }
        }

        return animsInThisRoom;
    }
}
