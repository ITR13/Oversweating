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
            var width = 150;
            GUILayout.BeginArea(new Rect(20 + (width + 5) * i, 80, width, 400));
            DisplayTerminal(i);
            GUILayout.EndArea();
        }
    }


    private void DisplayTerminal(int stationIndex)
    {
        var info = infos[stationIndex];
        if (info == null) return;
        var preset = Constants.Presets[info.preset_index];

        GUILayout.Label("Color: " + info.color);
        GUILayout.Label("Preset: " + info.preset_index);
        GUILayout.Label("Status: " + info.status);

        GUILayout.Space(1);
        GUILayout.Label("Components");
        for (var i = 0; i < info.components.Length; i++)
        {
            var buttonCount = preset[i] == Component.Lever ? 3 : 1;

            GUILayout.BeginHorizontal();
            GUILayout.Label(preset[i].ToString());

            for (var j = 0; j < buttonCount; j++)
            {
                if (GUILayout.Button(info.components[i].ToString()))
                {
                    NetworkManager.Instance.Activate(stationIndex, i, j);
                }
            }
            GUILayout.EndHorizontal();
        }

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
