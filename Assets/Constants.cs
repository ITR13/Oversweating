using System.Collections.Generic;

public static class Constants
{
    public const int LeverMax = 3;

    public static readonly Component[][] Presets = new[]
    {
        new[] { Component.Button, Component.Button, Component.Button, Component.Switch, Component.Lever, Component.Lever, Component.Compass },
    };

    public static Dictionary<StationStatus, string> StatusStrings =
        new Dictionary<StationStatus, string>
        {
            {StationStatus.Stopped, "stopped"},
            {StationStatus.Disabled, "disabled"},
            {StationStatus.Running, "running"},
            {StationStatus.Warning, "warning"},
            {StationStatus.Failed, "failed"},
        };
}

public enum StationStatus
{
    Stopped,
    Disabled,
    Running,
    Warning,
    Failed,
}

public enum Component
{
    Button,
    Lever,
    Switch,
    Compass,
}
