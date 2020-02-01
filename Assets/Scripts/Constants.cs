using System;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const int LeverMax = 3;

    public static readonly Component[][] Presets = new[]
    {
        new[] { Component.Button, Component.Button, Component.Button, Component.Switch, Component.Lever, Component.Lever, Component.Compass },
        new[] { Component.Button, Component.Button, Component.Compass, Component.Compass, Component.Compass },
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
    
    public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0);


    public static int ColorCount => Pallettes.Count;

    public static Dictionary<string, Pallette> Pallettes =
        new Dictionary<string, Pallette>
        {
            {"pink", new Pallette(0, "#FF9FF3", "#F368E0")},
            {"yellow", new Pallette(1, "#FECA57", "#FF9F43")},
            {"red", new Pallette(2, "#FF6B6B", "#EE5253")},
            {"blue", new Pallette(3, "#48DBFB", "#0ABDE3")},
            {"purple", new Pallette(4, "#8768DC", "#7D32EE")},
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

public struct Pallette
{
    public int Index;
    public Color BackgroundColor;
    public Color TintColor;

    public Pallette(int index, string backgroundHex, string tintHex)
    {
        Index = index;
        ColorUtility.TryParseHtmlString(backgroundHex, out BackgroundColor);
        ColorUtility.TryParseHtmlString(tintHex, out TintColor);
    }
}