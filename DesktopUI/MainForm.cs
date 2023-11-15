using System.IO.Ports;

namespace DesktopUI;

public partial class MainForm : Form
{
    private readonly List<Direction> _sequence = new();
    private readonly Random _random = new();
    private readonly int _directionsCount;
    private readonly int _delay = 1000;

    public MainForm()
    {
        InitializeComponent();

        // TODO: Uncomment
        // Must be unabled until connected
        //SetEnable(false, StartButton, RepeatButton, PortSelector, MaxTactsEdit);

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
        TactProgressLabel.Invoke(() => TactProgressLabel.Text = $"{currentTact + 1}/{tacts}");
    }

    private void StartSlideShow()
    {
        var tacts = (int)MaxTactsEdit.Value;
        for (int i = 0; i < tacts; i++)
        {
            InvokeSetImage(_sequence[i]);
            InvokeSetProgress(i, tacts);
            Thread.Sleep(_delay);
        }

        InvokeSetImage(Direction.Error);
    }
    #endregion


    #region Handlers

    private void StartButton_Click(object sender, EventArgs e)
    {
        SetEnable(false, StartButton, PortSelector, MaxTactsEdit);

        Task.Factory.StartNew(() =>
        {
            _sequence.Clear();
            var sequenceLength = (int)MaxTactsEdit.Value;
            for (int i = 0; i < sequenceLength; i++)
                _sequence.Add((Direction)_random.Next(0, _directionsCount));

            StartSlideShow();

            TactProgressLabel.Invoke(() => TactProgressLabel.Text = $"");
            InvokeSetEnable(true, StartButton, RepeatButton, PortSelector, MaxTactsEdit);
        });
    }

    private void RepeatButton_Click(object sender, EventArgs e)
    {
        SetEnable(false, StartButton, RepeatButton, PortSelector, MaxTactsEdit);

        Task.Factory.StartNew(() =>
        {
            StartSlideShow();

            TactProgressLabel.Invoke(() => TactProgressLabel.Text = $"");
            InvokeSetEnable(true, StartButton, RepeatButton, PortSelector, MaxTactsEdit);
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

            // TODO: Implement serial IO
            Thread.Sleep(1000);

            InvokeSetEnable(true, PortSelector, ConnectButton, RefreshButton);
        });
    }

    #endregion

}