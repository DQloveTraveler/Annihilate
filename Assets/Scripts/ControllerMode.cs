using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMode
{
    public enum Controller
    {
        MouseAndKey, GamePad
    }

    public static bool IsGamePad
    {
        get { return Mode == Controller.GamePad; }
    }

    public static bool IsMouseAndKey
    {
        get { return Mode == Controller.MouseAndKey; }
    }

    public static Controller Mode { get; set; } = Controller.MouseAndKey;
}
