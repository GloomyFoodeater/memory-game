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
        SetEnable(false, StartButton, MaxTactsEdit);

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
        PortSelector.Items.Clear();
        PortSelector.Items.AddRange(ports);

        if (ports.Any())
        {
            PortSelector.SelectedIndex = 0;
            ConnectButton.Enabled = true;
        }
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
            _gameState.IsStarted = true;
        });
    }

    private void MaxTactsEdit_ValueChanged(object sender, EventArgs e) => _gameState.FullReset();

    private void RefreshButton_Click(object sender, EventArgs e) => ListPorts();

    private void ConnectButton_Click(object sender, EventArgs e) => Task.Factory.StartNew(() =>
    {
        InvokeSetEnable(false, PortSelector, ConnectButton, RefreshButton);

        var portName = PortSelector.Invoke(() => PortSelector.Items[PortSelector.SelectedIndex].ToString());
        var baudRate = 115200;

        try
        {
            using var port = new SerialPort(portName, baudRate);
            port.Open();
            InvokeSetEnable(true, StartButton, MaxTactsEdit);
            var mustDiscard = true;
            var prevDirection = Direction.Error;
            var repeatCount = 0;

            while (port.IsOpen)
            {
                if (_gameState.IsStarted)
                {
                    if (mustDiscard)
                    {
                        port.DiscardOutBuffer();
                        port.DiscardInBuffer();
                        mustDiscard = false;
                        InvokeSetEnable(false, StartButton, MaxTactsEdit);
                    }

                    var message = port.ReadLine();
                    (_gameState.Direction, _gameState.InputType) = ParseMessage(message);

                    if (_gameState.Direction == Direction.Error || _gameState.InputType == InputType.Error) continue;
                    if (_gameState.InputType == InputType.Joystick)
                    {
                        if (prevDirection != _gameState.Direction)
                        {
                            prevDirection = _gameState.Direction;
                            repeatCount = 0;
                        }
                        else repeatCount++;

                        if (repeatCount < 5) continue;

                        prevDirection = Direction.Error;
                        repeatCount = 0;
                    }


                    if (_gameState.Direction == _gameState.Sequence[_gameState.CorrectGuesses])
                    {
                        _gameState.CorrectGuesses++;
                        Output();

                        if (_gameState.IsWon)
                        {
                            MessageBox.Show($"Won: {_gameState.ProgressString}!", "Success");
                            _gameState.FullReset();
                            Output();
                            mustDiscard = true;
                            InvokeSetEnable(true, StartButton, PortSelector, MaxTactsEdit);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Try again", "Failure");
                        _gameState.FullReset();
                        Output();
                        mustDiscard = true;
                        InvokeSetEnable(true, StartButton, PortSelector, MaxTactsEdit);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, $"Error: {ex.GetType().Name}");
        }

        _gameState.FullReset();

        InvokeSetEnable(false, StartButton, MaxTactsEdit);

        InvokeSetEnable(true, PortSelector, ConnectButton, RefreshButton);
    });

    #endregion

}