using static System.Windows.Forms.AxHost;

namespace DesktopUI;

internal class GameState
{
    public GameState() => FullReset();

    public List<Direction> Sequence = new();

    public int CorrectGuesses;
    public Direction Direction;
    public InputType InputType;
    public Direction PrevDirection;
    public InputType PrevInputType;
    public int JoystickRepetitions;
    public bool IsStarted;

    public bool IsWon => CorrectGuesses == Sequence.Count;

    public string ProgressString => $"{CorrectGuesses}/{Sequence.Count}";

    public void ResetProgress()
    {
        ResetJoystickData();

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

    public void ResetJoystickData()
    {
        JoystickRepetitions = 0;
        PrevDirection = Direction.Error;
        PrevInputType = InputType.Error;
    }
}
