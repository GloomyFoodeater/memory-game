namespace DesktopUI;

internal static class Parsing
{
    public static Direction ParseJoystickInput(string input)
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

    public static Direction ParseButtonsInput(string input)
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

    public static (Direction, InputType) ParseMessage(string message)
    {
        // Length is larger than lenght of "bt::bt" or "js::js"
        if (message.Length < 7) return (Direction.Error, InputType.Error);

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
}
