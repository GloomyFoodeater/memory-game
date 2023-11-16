using System.IO.Ports;
using System.Windows.Forms;

namespace DesktopUI;

public partial class MainForm : Form
{
    private readonly List<Direction> _sequence = new();
    private readonly Random _random = new();
    private readonly int _directionsCount;
    private readonly int _delay = 1000;
    private bool _ignoreInput = true;

    public MainForm()
    {
        InitializeComponent();

        // TODO: Uncomment
        // Must be unabled until connected
        // InvokeSetEnable(false, StartButton, RepeatButton, PortSelector, MaxTactsEdit);

        ListPorts();
        _directionsCount = Enum.GetValues(typeof(Direction)).Length - 1; // -1 for unknown
        TactProgressLabel.Text = "";
    }

    #region Helpers

    private void ListPorts()
    {
        var ports = SerialPort.GetPortNames();
        PortSelector.Items.AddRange(ports);
        PortSelector.SelectedIndex = ports.Length > 0 ? 1 : -1;

        if (!ports.Any()) ConnectButton.Enabled = false;
    }

    private static void SetEnable(bool value, params Control[] controls)
    {
        foreach (var control in controls)
            control.Enabled = value;
    }

    private static void InvokeSetEnable(bool value, params Control[] controls)
    {
        foreach (var control in controls)
            control.Invoke(() => control.Enabled = value);
    }

    private void InvokeSetImage(Direction dir)
    {
        if (dir == Direction.Error)
        {
            DirectionPictureBox.Invoke(() => DirectionPictureBox.Image = null);
            return;
        }

        var enumName = Enum.GetName(typeof(Direction), dir);
        DirectionPictureBox.Invoke(() => DirectionPictureBox.Image = Image.FromFile($"./Assets/{dir}.png"));
    }

    private void InvokeSetProgress(int currentTact, int tacts)
    {
        TactProgressLabel.Invoke(() => TactProgressLabel.Text = $"{currentTact}/{tacts}");
    }

    private void StartSlideShow()
    {
        InvokeSetImage(Direction.Error);

        var tacts = _sequence.Count;
        for (int i = 0; i < tacts; i++)
        {
            InvokeSetImage(_sequence[i]);
            InvokeSetProgress(i + 1, tacts);
            Thread.Sleep(_delay);
        }

        InvokeSetImage(Direction.Error);
    }

    private static Direction ParseJoystickInput(string input)
    {
        try
        {
            int delta = 200;
            string[] words = input.Split(',');
            int x = int.Parse(words[1]);
            int y = int.Parse(words[2]);
            if ((x < (2000 + delta) && (x > (2000 - delta)) && (y < delta)))
            {
                return Direction.Down;
            }
            else if ((y < (2000 + delta) && (y > (2000 - delta)) && (x < delta)))
            {
                return Direction.Left;
            }
            else if ((x < (2000 + delta) && (x > (2000 - delta)) && (y < (4000 + delta)) && (y > (4000 - delta))))
            {
                return Direction.Up;
            }
            else if ((x < (4000 + delta) && (x > (4000 - delta)) && (y < (2000 + delta)) && (y > (2000 - delta))))
            {
                return Direction.Right;
            }
            return Direction.Error;
        }
        catch
        {
            return Direction.Error;
        }
    }

    private static Direction ParseButtonsInput(string input)
    {
        return input switch
        {
            "D" => Direction.Down,
            "U" => Direction.Up,
            "L" => Direction.Left,
            "R" => Direction.Right,
            _ => Direction.Error,
        };
    }

    private static (Direction, InputType) ParseMessage(string message)
    {
        // Length is larger than lenght of "bt::bt" or "js::js"
        if(message.Length < 7) return (Direction.Error, InputType.Error);

        var inputType = message[..2] switch
        {
            "js" => InputType.Joystick,
            "bt" => InputType.Buttons,
            _ => InputType.Error
        };

        if (inputType == InputType.Error) return (Direction.Error, InputType.Error);

        var input = message[3..^3];
        var direction = inputType switch
        {
            InputType.Joystick => ParseJoystickInput(input),
            InputType.Buttons => ParseButtonsInput(input),
            _ => Direction.Error
        };

        return (direction, inputType);
    }

    private static bool CheckJoystickSensitivity(InputType inputType, Direction direction, ref Direction prevDirection, ref int repetitionCount)
    {
        if (inputType == InputType.Joystick)
        {
            if (prevDirection != direction)
            {
                prevDirection = direction;
                repetitionCount = 0;
                return false;
            }
            else repetitionCount++;
        }
        return repetitionCount < 5;
    }

    private void ResetGameProgress(out Direction prevDirection, out int repetitionCount, out int correctGuesses, int sequenceLength, Direction direction)
    {
        direction = prevDirection = Direction.Error;
        repetitionCount = correctGuesses = 0;
        InvokeSetImage(direction);
        InvokeSetProgress(correctGuesses, sequenceLength);
    }

    #endregion

    #region Handlers

    private void StartButton_Click(object sender, EventArgs e)
    {
        SetEnable(false, StartButton, PortSelector, MaxTactsEdit);

        _ignoreInput = true;
        Task.Factory.StartNew(() =>
        {
            _sequence.Clear();
            var sequenceLength = (int)MaxTactsEdit.Value;
            for (int i = 0; i < sequenceLength; i++)
                _sequence.Add((Direction)_random.Next(0, _directionsCount));

            StartSlideShow();

            InvokeSetProgress(0, _sequence.Count);
            InvokeSetEnable(true, StartButton, RepeatButton, PortSelector, MaxTactsEdit);

            _ignoreInput = false;
        });
    }

    private void RepeatButton_Click(object sender, EventArgs e)
    {
        SetEnable(false, StartButton, RepeatButton, PortSelector, MaxTactsEdit);

        _ignoreInput = true;
        Task.Factory.StartNew(() =>
        {
            StartSlideShow();

            InvokeSetProgress(0, _sequence.Count);
            InvokeSetEnable(true, StartButton, RepeatButton, PortSelector, MaxTactsEdit);

            _ignoreInput = false;
        });
    }

    private void MaxTactsEdit_ValueChanged(object sender, EventArgs e)
    {
        RepeatButton.Enabled = false;
        _sequence.Clear();
    }

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

                var prevDirection = Direction.Error;
                var repetitionCount = 0;
                var correctGuesses = 0;

                while (port.IsOpen)
                {
                    if (_ignoreInput) continue;

                    InvokeSetEnable(false, StartButton, PortSelector, MaxTactsEdit);

                    var sequenceLength = _sequence.Count;

                    var message = port.ReadLine();
                    var (direction, inputType) = ParseMessage(message);

                    // Process joystick sensitivity
                    var isJoystickEvent = CheckJoystickSensitivity(inputType, direction, ref prevDirection, ref repetitionCount);
                    if (!isJoystickEvent) continue;

                    prevDirection = Direction.Error;
                    repetitionCount = 0;

                    // Process actual input sequence
                    if (direction == _sequence[correctGuesses])
                    {
                        correctGuesses++;
                        InvokeSetImage(direction);
                        InvokeSetProgress(correctGuesses, sequenceLength);

                        if (correctGuesses == sequenceLength)
                        {
                            MessageBox.Show("Success", $"Won: {correctGuesses}/{sequenceLength}!!!");
                            ResetGameProgress(out prevDirection, out repetitionCount, out correctGuesses, sequenceLength, direction);
                            InvokeSetEnable(true, StartButton, PortSelector, MaxTactsEdit);
                        }
                    }
                    else
                    {
                        ResetGameProgress(out prevDirection, out repetitionCount, out correctGuesses, sequenceLength, direction);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, $"Error: {ex.GetType().Name}");
            }

            InvokeSetEnable(false, StartButton, RepeatButton, PortSelector, MaxTactsEdit);
            InvokeSetEnable(true, PortSelector, ConnectButton, RefreshButton);
        });
    }

    #endregion

}