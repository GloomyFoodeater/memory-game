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

    public void ResetProgress()
    {
        ResetJoystick();

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

    public void ResetJoystick()
    {
        JoystickRepetitions = 0;
        PrevDirection = Direction.Error;
    }
}
