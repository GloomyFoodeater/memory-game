using DesktopUI.Core.Model;

namespace DesktopUI.Core;

public class MemoryGame
{
    private int _requestedLength;
    private Direction[]? _sequence;
    private int _currentIdx;
    private Direction _currentDir;

    public event Action? OnReset;
    public event Action<Direction[]>? OnGenerated;
    public event Action<Direction, int, int>? OnCorrectGuess;
    public event Action? OnFailure; 
    public event Action? OnSuccess;

    private GameState _state;
    private readonly Dictionary<GameState, Action> _transitionTable;

    public MemoryGame()
    {
        SignalReset();

        _transitionTable = new()
        {
            { GameState.Idle,           Idle },
            { GameState.Generate,       GenerateSequence },
            { GameState.PendingInput,   ProcessInput },
            { GameState.CorrectGuess,   ProcessCorrectGuess },
            { GameState.Success,        ProcessSuccess },
            { GameState.Failure,        ProcessFailure },
        };
    }

    public void SignalReset()
    {
        _requestedLength = 0;
        _currentIdx = 0;
        _currentDir = Direction.Error;
        _state = GameState.Idle;
        OnReset?.Invoke();
    }

    public void SignalGeneration(int size)
    {
        if (size <= 0) throw new ArgumentException();
        _currentIdx = 0;
        _requestedLength = size;

        _state = GameState.Generate;
    }

    public void SignalInput(Direction dir)
    {
        if (dir == Direction.Error) throw new ArgumentException();
        _currentDir = dir;

        _state = GameState.PendingInput;
    }

    public void Run()
    {
        do
        {
            var action = _transitionTable[_state];
            action();
        } while (true);
    }

    #region Transition actions
    private void Idle() { }

    private void GenerateSequence()
    {
        var random = new Random();

        var max = Enum.GetValues(typeof(Direction)).Length - 1; // -1 for error
        _sequence = new Direction[_requestedLength];
        for (int i = 0; i < _requestedLength; i++)
            _sequence[i] = ((Direction)random.Next(0, max));

        OnGenerated?.Invoke(_sequence);

        _state = GameState.Idle;
    }

    private void ProcessInput()
    {
        var match = _currentDir == _sequence![_currentIdx++];

        _state = match
            ? (_currentIdx == _requestedLength ? GameState.Success : GameState.CorrectGuess)
            : GameState.Failure;
    }

    private void ProcessCorrectGuess()
    {
        OnCorrectGuess?.Invoke(_currentDir, _currentIdx, _requestedLength);

        _state = GameState.Idle;
    }

    private void ProcessFailure()
    {
        OnFailure?.Invoke();

        _state = GameState.Idle;
    }

    private void ProcessSuccess()
    {
        OnCorrectGuess?.Invoke(_currentDir, _currentIdx, _requestedLength);
        OnSuccess?.Invoke();

        _state = GameState.Idle;
    }
    #endregion
}
