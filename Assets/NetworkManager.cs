﻿using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager : MonoBehaviour
{
    private const string Url = "http://192.168.137.1:5000/";


    public static NetworkManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public UnityWebRequest Setup(int stationCount, int playerCount)
    {
        var form = new WWWForm();
        form.AddField("station_count", stationCount);
        form.AddField("player_count", playerCount);
        var request = UnityWebRequest.Post($"{Url}setup", form);
        request.SendWebRequest();
        return request;
    }

    public UnityWebRequest Info(int station)
    {
        var request = UnityWebRequest.Get($"{Url}info/{station}");
        request.SendWebRequest();
        return request;
    }

    public UnityWebRequest Activate(int station, int component, int button)
    {
        var form = new WWWForm();
        form.AddField("component_index", component);
        form.AddField("button_index", button);
        var request = UnityWebRequest.Post($"{Url}activate/{station}", form);
 
         request.SendWebRequest();
        return request;
    }
}