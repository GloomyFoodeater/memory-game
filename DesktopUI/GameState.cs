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
    public int JoystickRepetitions;
    public bool IgnoreInput;

    public bool IsFinished => Direction != Sequence[CorrectGuesses] || CorrectGuesses == Sequence.Count;

    public bool IsWon => CorrectGuesses == Sequence.Count && Direction == Sequence[CorrectGuesses];

    public string ProgressString => $"{CorrectGuesses}/{Sequence.Count}";

    public bool IsJoystickEvent()
    {
        if (PrevDirection != Direction)
        {
            PrevDirection = Direction;
            JoystickRepetitions = 0;
            return false;
        }
        else JoystickRepetitions++;
        return JoystickRepetitions < 5;
    }

    public void ResetProgress()
    {
        ResetJoystickData();

        Direction = Direction.Error;
        CorrectGuesses = 0;
        InputType = InputType.Error;
    }

    public void FullReset()
    {
        Sequence.Clear();
        IgnoreInput = true;
        ResetProgress();
    }

    public void ResetJoystickData()
    {
        JoystickRepetitions = 0;
        PrevDirection = Direction.Error;
    }
}
