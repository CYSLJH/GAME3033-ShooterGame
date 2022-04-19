using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppEvents
{
    public static bool currentPause;

    public delegate void OnMouseCursorEnable(bool enabled);

    public static event OnMouseCursorEnable MouseCursorEnabled;

    public static void InvokeOnMouseCursorEnable(bool enabled)
    {
        MouseCursorEnabled?.Invoke(enabled);
    }

    public delegate void OnPauseEvent(bool enabled);
    public static event OnPauseEvent PauseEnabled;
    public static void InvokeOnPauseEvent(bool paused)
    {
        currentPause = paused;
        PauseEnabled?.Invoke(currentPause);
    }
}
