using static System.Windows.Forms.AxHost;

namespace DesktopUI;

internal class GameState
{
    public GameState() => FullReset();

    public List<Direction> Sequence = new();

    public int CorrectGuesses;
    public Direction Direction;
    public InputType InputType;
    public bool IsStarted;

    public bool IsWon => CorrectGuesses == Sequence.Count;

    public string ProgressString => $"{CorrectGuesses}/{Sequence.Count}";

    public void ResetProgress()
    {
        Direction = Direction.Error;
        InputType = InputType.Error;
        CorrectGuesses = 0;
    }

    public void FullReset()
    {
        Sequence.Clear();
        IsStarted = false;
        ResetProgress();
    }
}
