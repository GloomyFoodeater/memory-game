using System.IO.Ports;

using static DesktopUI.Parsing;
using static DesktopUI.ControlUtils;

namespace DesktopUI;

public partial class MainForm : Form
{
    private readonly GameState _state = new();
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

    private void ListPorts()
    {
        var ports = SerialPort.GetPortNames();
        PortSelector.Items.AddRange(ports);
        PortSelector.SelectedIndex = ports.Length > 0 ? 1 : -1;

        if (!ports.Any()) ConnectButton.Enabled = false;
    }

    private void StartSlideShow()
    {
        DirectionPictureBox.InvokeSetImage(Direction.Error);

        var tacts = _state.Sequence.Count;
        for (int i = 0; i < tacts; i++)
        {
            DirectionPictureBox.InvokeSetImage(_state.Sequence[i]);
            ProgressLabel.InvokeSetProgress(i + 1, tacts);
            Thread.Sleep(_delay);
        }

        DirectionPictureBox.InvokeSetImage(Direction.Error);
    }

    private bool IsJoystickEvent()
    {
        if (_state.InputType == InputType.Joystick)
        {
            if (_state.PrevDirection != _state.Direction)
            {
                _state.PrevDirection = _state.Direction;
                _state.JoystickRepetitions = 0;
                return false;
            }
            else _state.JoystickRepetitions++;
        }
        return _state.JoystickRepetitions < 5;
    }

    #endregion

    #region Handlers

    private void StartButton_Click(object sender, EventArgs e)
    {
        SetEnable(false, StartButton, RepeatButton, MaxTactsEdit);
        _state.FullReset();

        Task.Factory.StartNew(() =>
        {
            var sequenceLength = (int)MaxTactsEdit.Value;
            var max = Enum.GetValues(typeof(Direction)).Length - 1; // -1 for unknown
            for (int i = 0; i < sequenceLength; i++)
                _state.Sequence.Add((Direction)_random.Next(0, max));

            StartSlideShow();
            ProgressLabel.InvokeSetProgress(0, sequenceLength);
            InvokeSetEnable(true, StartButton, RepeatButton, MaxTactsEdit);
            _state.IgnoreInput = false;
        });
    }

    private void RepeatButton_Click(object sender, EventArgs e)
    {
        SetEnable(false, StartButton, RepeatButton, MaxTactsEdit);
        _state.IgnoreInput = true;
        _state.ResetProgress();

        Task.Factory.StartNew(() =>
        {
            StartSlideShow();
            ProgressLabel.InvokeSetProgress(0, _state.Sequence.Count);
            InvokeSetEnable(true, StartButton, RepeatButton, MaxTactsEdit);
            _state.IgnoreInput = false;
        });
    }

    private void MaxTactsEdit_ValueChanged(object sender, EventArgs e) => _state.FullReset();

    private void RefreshButton_Click(object sender, EventArgs e) => ListPorts();

    private void ConnectButton_Click(object sender, EventArgs e)
    {
        Task.Factory.StartNew(() =>
        {
            InvokeSetEnable(false, PortSelector, ConnectButton, RefreshButton);

            var portName = PortSelector.SelectedText;
            var baudRate = 115200;

            try
            {
                using var port = new SerialPort(portName, baudRate);
                port.Open();

                while (port.IsOpen)
                {
                    if (_state.IgnoreInput) continue;

                    InvokeSetEnable(false, StartButton, MaxTactsEdit);

                    var message = port.ReadLine();
                    (_state.Direction, _state.InputType) = ParseMessage(message);

                    // Process joystick sensitivity
                    if (!IsJoystickEvent()) continue;
                    _state.ResetJoystick();

                    // Process actual input sequence
                    if (_state.Direction == _state.Sequence[_state.CorrectGuesses])
                    {
                        _state.CorrectGuesses++;
                        DirectionPictureBox.InvokeSetImage(_state.Direction);
                        ProgressLabel.InvokeSetProgress(_state.CorrectGuesses, _state.Sequence.Count);

                        if (_state.CorrectGuesses == _state.Sequence.Count)
                        {
                            MessageBox.Show("Success", $"Won: {_state.CorrectGuesses}/{_state.Sequence.Count}!!!");

                            _state.FullReset();
                            DirectionPictureBox.InvokeSetImage(Direction.Error);
                            ProgressLabel.InvokeSetProgress(0, _state.Sequence.Count);

                            InvokeSetEnable(true, StartButton, PortSelector, MaxTactsEdit);
                        }
                    }
                    else
                    {
                        _state.ResetProgress();
                        DirectionPictureBox.InvokeSetImage(Direction.Error);
                        ProgressLabel.InvokeSetProgress(0, _state.Sequence.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, $"Error: {ex.GetType().Name}");
            }

            InvokeSetEnable(false, StartButton, RepeatButton, MaxTactsEdit);

            InvokeSetEnable(true, PortSelector, ConnectButton, RefreshButton);
        });
    }

    #endregion

}