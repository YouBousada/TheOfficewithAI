using System.Collections.Generic;

public interface IAnimatronicRegistry
{
    IEnumerable<Animatronic> Animatronics { get; }
    bool TryGetAnimatronic(CharacterName character, out Animatronic animatronic);
}
