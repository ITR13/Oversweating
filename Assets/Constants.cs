public static class Constants
{
    public const int LeverMax = 3;

    public static readonly Component[][] Presets = new[]
    {
        new[] { Component.Button, Component.Button, Component.Button, Component.Switch, Component.Lever, Component.Lever, Component.Compass },
    };
}

public enum Playstate
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
