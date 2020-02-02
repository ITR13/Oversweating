using UnityEngine;
using UnityEngine.Networking;
using NetCall = System.Tuple<UnityEngine.Networking.UnityWebRequest, UnityEngine.Networking.UnityWebRequestAsyncOperation>;

public class NetworkManager : MonoBehaviour
{
    private const string Url = "http://192.168.137.1:5000/";

    public static NetworkManager Instance { get; private set; }
    public static bool InstanceSet { get; private set; }

    private void Update()
    {
        if (!InstanceSet)
        {
            InstanceSet = true;
            Instance = this;
        }
    }

    public NetCall Setup(int stationCount, int playerCount)
    {
        var form = new WWWForm();
        form.AddField("station_count", stationCount);
        form.AddField("player_count", playerCount);
        var request = UnityWebRequest.Post($"{Url}setup", form);
        var asyncOperation = request.SendWebRequest();
        return new NetCall(request, asyncOperation);
    }

    public NetCall Info(int station)
    {
        var request = UnityWebRequest.Get($"{Url}info/{station}");
        var asyncOperation = request.SendWebRequest();
        return new NetCall(request, asyncOperation);
    }

    public NetCall Activate(int station, int component, int button)
    {
        var form = new WWWForm();
        form.AddField("component_index", component);
        form.AddField("button_index", button);
        var request = UnityWebRequest.Post($"{Url}activate/{station}", form);

        var asyncOperation = request.SendWebRequest();
        return new NetCall(request, asyncOperation);
    }

    public NetCall Stop()
    {
        var request = UnityWebRequest.Get($"{Url}stop");
        var asyncOperation = request.SendWebRequest();
        return new NetCall(request, asyncOperation);
    }
    public NetCall Ready(int station)
    {
        var request = UnityWebRequest.Get($"{Url}ready/{station}");
        var asyncOperation = request.SendWebRequest();
        return new NetCall(request, asyncOperation);
    }
}
