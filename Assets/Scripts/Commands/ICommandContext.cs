public interface ICommandContext
{
    Player Player { get; }
    Office OfficeStates { get; }
    PuzzleMaster puzzleMaster { get; }
}
