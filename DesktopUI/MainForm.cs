using DesktopUI.Core;
using DesktopUI.Core.Model;
using System.IO.Ports;
using static DesktopUI.ControlUtils;

namespace DesktopUI;

public partial class MainForm : Form
{
    private readonly MemoryGame _game = new();
    bool isGenerated = false;
    bool isKeyboard = true;

    public MainForm()
    {
        InitializeComponent();

        _game.OnGenerated += OnGenerated;
        _game.OnCorrectGuess += Output;
        _game.OnFailure += OnFailure;
        _game.OnSuccess += OnSuccess;

        Task.Factory.StartNew(_game.Run);
        ListPorts();
    }

    #region Helpers
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

    private void OnGenerated(Direction[] sequence)
    {
        isGenerated = false;

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
        Output(Direction.Error, default, default);

        InvokeSetEnable(true, MaxTactsEdit, StartButton);
    }

    private void OnSuccess()
    {
        MessageBox.Show("Yay!", "Success!");
        Output(Direction.Error, default, default);
        InvokeSetEnable(true, MaxTactsEdit, StartButton);
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
        Task.Factory.StartNew(() =>
        {

        });
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
            default: break;
        }
    }

    #endregion
}