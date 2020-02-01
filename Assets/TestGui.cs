using System.Collections;
using UnityEngine;

class TestGui : MonoBehaviour
{
    private string _stationCount = "5", _playerCount = "2";
    private StationInfo[] infos = new StationInfo[5];

    private void Start()
    {
        for (var i = 0; i < infos.Length; i++)
        {
            StartCoroutine(StartPolling(i));
        }
    }


    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, 200, 50));
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Setup"))
        {
            NetworkManager.Instance.Setup(
                int.Parse(_stationCount),
                int.Parse(_playerCount)
            );
        }

        GUILayout.Label("Stations");
        _stationCount = GUILayout.TextField(_stationCount);
        GUILayout.Label("Players");
        _playerCount = GUILayout.TextField(_playerCount);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        for (var i = 0; i < infos.Length; i++)
        {
            GUILayout.BeginArea(new Rect(20 + 100 * i, 80, 100, 300));
            DisplayTerminal(i);
            GUILayout.EndArea();
        }
    }


    private void DisplayTerminal(int stationIndex)
    {
        var info = infos[stationIndex];
        if (info == null) return;
        GUILayout.Label("Color: " + info.color);
        GUILayout.Label("Preset: " + info.preset_index);
        GUILayout.Label("Status: " + info.status);


        //public int[] components;
        //public Tuple<int, int[][]>[] faults;

    }


    private IEnumerator StartPolling(int stationIndex)
    {
        while (true)
        {
            var request = NetworkManager.Instance.Info(stationIndex);
            while (request.downloadProgress < 1)
            {
                yield return null;
            }

            var json = request.downloadHandler.text;
            var info = JsonUtility.FromJson<StationInfo>(json);
            infos[stationIndex] = info;
        }
    }
}
