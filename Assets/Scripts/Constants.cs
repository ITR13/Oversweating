using System;
using System.Collections.Generic;
using UnityEngine;
using IconIndex = System.Tuple<string, int>;
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

    public static readonly Dictionary<string, Pallette> Pallettes =
        new Dictionary<string, Pallette>
        {
            {"pink", new Pallette(0, "#FF9FF3", "#F368E0")},
            {"yellow", new Pallette(1, "#FECA57", "#FF9F43")},
            {"red", new Pallette(2, "#FF6B6B", "#EE5253")},
            {"blue", new Pallette(3, "#48DBFB", "#0ABDE3")},
            {"purple", new Pallette(4, "#8768DC", "#7D32EE")},
        };

    public static Dictionary<IconIndex, Sprite> InstructionIcons = new Dictionary<IconIndex, Sprite>
    {
        {new IconIndex("button", 0), Resources.Load<Sprite>("Icons/Button_Off") },
        {new IconIndex("button", 1), Resources.Load<Sprite>("Icons/Button_On") },

        {new IconIndex("compass", 0), Resources.Load<Sprite>("Icons/North") },
        {new IconIndex("compass", 1), Resources.Load<Sprite>("Icons/East") },
        {new IconIndex("compass", 2), Resources.Load<Sprite>("Icons/South") },
        {new IconIndex("compass", 3), Resources.Load<Sprite>("Icons/West") },

        {new IconIndex("lever", 0), Resources.Load<Sprite>("Icons/Lever") },
        {new IconIndex("lever", 1), Resources.Load<Sprite>("Icons/Lever_Up") },
        {new IconIndex("lever", 2), Resources.Load<Sprite>("Icons/Lever_Down") },

        {new IconIndex("switch", 0), Resources.Load<Sprite>("Icons/On") },
        {new IconIndex("switch", 1), Resources.Load<Sprite>("Icons/Off") },
    };

    static Constants()
    {
        var stationIcons = Resources.LoadAll<Sprite>("StationIconSheet");
        for (var i = 0; i < stationIcons.Length; i++)
        {
            InstructionIcons.Add(new IconIndex("station", i), stationIcons[i]);
        }

        foreach (var pair in InstructionIcons)
        {
            if (pair.Value != null) continue;
            Debug.LogError($"Failed to find image for {pair.Key}");
        }
    }
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