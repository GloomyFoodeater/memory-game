using DesktopUI.Core.Model;

namespace DesktopUI;

internal static class ControlUtils
{
    public static void SetEnable(bool value, params Control[] controls)
    {
        foreach (var control in controls)
            control.Enabled = value;
    }

    public static void InvokeSetEnable(bool value, params Control[] controls)
    {
        foreach (var control in controls)
            control.Invoke(() => control.Enabled = value);
    }

    public static void InvokeSetImage(this PictureBox pictureBox, Direction dir)
    {
        if (dir == Direction.Error)
        {
            pictureBox.Invoke(() => pictureBox.Image = null);
            return;
        }

        var enumName = Enum.GetName(typeof(Direction), dir);
        pictureBox.Invoke(() => pictureBox.Image = Image.FromFile($"./Assets/{dir}.png"));
    }

    public static void InvokeSetText(this Label label, string progressString)
    {
        label.Invoke(() => label.Text = progressString);
    }

}
