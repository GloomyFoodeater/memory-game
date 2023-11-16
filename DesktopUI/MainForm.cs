using System.IO.Ports;

using static DesktopUI.Parsing;
using static DesktopUI.ControlUtils;

namespace DesktopUI;

public partial class MainForm : Form
{
    private readonly GameState _gameState = new();
    private readonly Random _random = new();
    private const int _delay = 1000;

    public MainForm()
    {
        InitializeComponent();

        // TODO: Uncomment
        // Must be unabled until connected
        // InvokeSetEnable(false, StartButton, RepeatButton, MaxTactsEdit);

        ListPorts();
    }

    #region Helpers

    private void Output()
    {
        DirectionPictureBox.InvokeSetImage(_gameState.Direction);
        ProgressLabel.InvokeSetText(_gameState.ProgressString);
    }

    private void ListPorts()
    {
        var ports = SerialPort.GetPortNames();
        PortSelector.Items.AddRange(ports);
        PortSelector.SelectedIndex = ports.Length > 0 ? 1 : -1;

        if (!ports.Any()) ConnectButton.Enabled = false;
    }

    private void StartSlideShow()
    {
        _gameState.ResetProgress();
        Output();

        foreach (var dir in _gameState.Sequence)
        {
            _gameState.CorrectGuesses++;
            _gameState.Direction = dir;
            Output();
            Thread.Sleep(_delay);
        }

        _gameState.ResetProgress();
        Output();

    }

    private void ProcessMessage(SerialPort port)
    {
        InvokeSetEnable(false, StartButton, MaxTactsEdit);

        var message = port.ReadLine();
        (_gameState.Direction, _gameState.InputType) = ParseMessage(message);

        // Process joystick sensitivity
        if (_gameState.InputType == InputType.Joystick && !_gameState.IsJoystickEvent())
            _gameState.ResetJoystickData();

        if (_gameState.IsFinished)
        {
            var caption = _gameState.IsWon ? "Success" : "Failure";
            var text = _gameState.IsWon ? $"Won: {_gameState.ProgressString}!!!" : "Try again!";

            MessageBox.Show(text, caption);

            _gameState.FullReset();
            Output();

            InvokeSetEnable(true, StartButton, PortSelector, MaxTactsEdit);
        }
        else
        {
            _gameState.CorrectGuesses++;
            Output();
        }
    }

    #endregion

    #region Handlers

    private void StartButton_Click(object sender, EventArgs e)
    {
        SetEnable(false, StartButton, MaxTactsEdit);
        _gameState.FullReset();

        Task.Factory.StartNew(() =>
        {
            var sequenceLength = (int)MaxTactsEdit.Value;
            var max = Enum.GetValues(typeof(Direction)).Length - 1; // -1 for unknown
            for (int i = 0; i < sequenceLength; i++)
                _gameState.Sequence.Add((Direction)_random.Next(0, max));

            StartSlideShow();

            InvokeSetEnable(true, StartButton, MaxTactsEdit);
            _gameState.IgnoreInput = false;
        });
    }

    private void MaxTactsEdit_ValueChanged(object sender, EventArgs e) => _gameState.FullReset();

    private void RefreshButton_Click(object sender, EventArgs e) => ListPorts();

    private void ConnectButton_Click(object sender, EventArgs e) => Task.Factory.StartNew(() =>
    {
        InvokeSetEnable(false, PortSelector, ConnectButton, RefreshButton);

        var portName = PortSelector.SelectedText;
        var baudRate = 115200;

        try
        {
            using var port = new SerialPort(portName, baudRate);
            port.Open();

            while (port.IsOpen)
                if (!_gameState.IgnoreInput)
                    ProcessMessage(port);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, $"Error: {ex.GetType().Name}");
        }

        InvokeSetEnable(false, StartButton, MaxTactsEdit);

        InvokeSetEnable(true, PortSelector, ConnectButton, RefreshButton);
    });

    #endregion

}