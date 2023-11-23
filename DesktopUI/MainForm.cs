using DesktopUI.Core;
using DesktopUI.Core.Model;
using System.IO.Ports;
using static DesktopUI.ControlUtils;

namespace DesktopUI;

public partial class MainForm : Form
{
    const int jsDelta = 100;
    const int jsMax = 4096;
    const int jsCenter = jsMax / 2;
    const int baudRate = 115200;

    private readonly MemoryGame _game = new();
    private bool isGenerated = false;
    private bool isKeyboard = false;

    public MainForm()
    {
        InitializeComponent();

        ConnectionContainer.Visible = !isKeyboard;

        _game.OnReset += OnReset;
        _game.OnGenerated += OnGenerated;
        _game.OnCorrectGuess += Output;
        _game.OnFailure += OnFailure;
        _game.OnFailure += OnReset;
        _game.OnSuccess += OnSuccess;
        _game.OnSuccess += OnReset;

        Task.Factory.StartNew(_game.Run);
        ListPorts();
    }

    #region Helpers
    private void ListPorts()
    {
        var ports = SerialPort.GetPortNames();
        PortSelector.Items.Clear();
        PortSelector.Items.AddRange(ports);

        ConnectButton.Enabled = ports.Any();
        if (ports.Any()) PortSelector.SelectedIndex = 0;
    }

    private void OnReset()
    {
        isGenerated = false;
        Output(Direction.Error, default, default);

        InvokeSetEnable(true, MaxTactsEdit, StartButton);
    }

    private void OnGenerated(Direction[] sequence)
    {
        Output(Direction.Error, 0, sequence.Length);

        for (int i = 0; i < sequence.Length; i++)
        {
            Output(sequence[i], i + 1, sequence.Length);
            Thread.Sleep(1000);
        }

        Output(Direction.Error, 0, sequence.Length);

        isGenerated = true;
    }

    private void Output(Direction dir, int idx, int length)
    {
        DirectionPictureBox.InvokeSetImage(dir);
        var progressString = length > 0 ? $"{idx}/{length}" : "";
        ProgressLabel.InvokeSetText(progressString);
    }

    private void OnFailure()
    {
        MessageBox.Show("Try again!", "Failure");
    }

    private void OnSuccess()
    {
        MessageBox.Show("Yay!", "Success!");
    }

    void OnJoystickMessage(string message, ref bool wasCenterDetected)
    {
        string[] words = message[3..].Split(',');
        int x = int.Parse(words[0]);
        int y = int.Parse(words[1]);

        var isCenter = Math.Abs(x - jsCenter) < jsDelta && Math.Abs(y - jsCenter) < jsDelta;
        if (isCenter)
        {
            wasCenterDetected = true;
            return;
        }

        Direction dir;
        if (y < jsDelta)
            dir = Direction.Down;
        else if (x < jsDelta)
            dir = Direction.Left;
        else if (y > jsMax - jsDelta)
            dir = Direction.Up;
        else if (x > jsMax - jsDelta)
            dir = Direction.Right;
        else
            return;

        if (wasCenterDetected)
        {
            wasCenterDetected = false;
            _game.SignalInput(dir);
        }
    }

    void OnButtonMessage(string message)
    {
        var dir = message[3..] switch
        {
            "D" => Direction.Down,
            "U" => Direction.Up,
            "R" => Direction.Right,
            "L" => Direction.Left,
            _ => Direction.Error
        };
        _game.SignalInput(dir);
    }

    private void ConnectionTask(string portName)
    {

        bool wasJsCenterDetected = false;

        try
        {
            using var port = new SerialPort(portName, baudRate);
            port.Open();
            ConnectionContainer.Invoke(() => ConnectionContainer.Visible = false);
            while (port.IsOpen)
            {
                if (!isGenerated) continue;
                var message = port.ReadLine();
                switch (message[..2])
                {
                    case "js": OnJoystickMessage(message, ref wasJsCenterDetected); break;
                    case "bt": OnButtonMessage(message); break;
                    default: throw new IOException();
                }
            }
        }
        catch (Exception e)
        {
            MessageBox.Show(e.Message, "Error");
            ConnectionContainer.Invoke(() => ConnectionContainer.Visible = true);
            _game.SignalReset();
        }
    }

    #endregion

    #region Handlers

    private void StartButton_Click(object sender, EventArgs e)
    {
        var size = (int)MaxTactsEdit.Value;
        _game.SignalGeneration(size);
        SetEnable(false, MaxTactsEdit, StartButton);
    }

    private void RefreshButton_Click(object sender, EventArgs e)
    {
        ListPorts();
    }

    private void ConnectButton_Click(object sender, EventArgs e)
    {
        try
        {
            isKeyboard = false;
            var portName = PortSelector.Items[PortSelector.SelectedIndex].ToString();
            Task.Factory.StartNew(() => ConnectionTask(portName));
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error");
        }
    }

    private void MainForm_KeyPress(object sender, KeyEventArgs e)
    {
        if (!isGenerated || !isKeyboard) return;

        switch (e.KeyCode)
        {
            case Keys.W: _game.SignalInput(Direction.Up); break;
            case Keys.A: _game.SignalInput(Direction.Left); break;
            case Keys.S: _game.SignalInput(Direction.Down); break;
            case Keys.D: _game.SignalInput(Direction.Right); break;
            case Keys.Escape:
                {
                    _game.SignalReset();
                    ConnectionContainer.Visible = true;
                }
                break;
            default: break;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        isKeyboard = true;
        ConnectionContainer.Visible = false;
    }
    #endregion
}