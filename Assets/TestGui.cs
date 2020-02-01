using System;
using System.Collections;
using UnityEngine;

class TestGui : MonoBehaviour
{
    private string _stationCount = "5", _playerCount = "2";
    private StationInfo[] infos = new StationInfo[5];
    private float[] timers = new float[5];

    private void Start()
    {
        for (var i = 0; i < infos.Length; i++)
        {
            StartCoroutine(StartPolling(i));
        }
    }

    private void Update()
    {
        for (var i = 0; i < timers.Length; i++)
        {
            if (infos[i] == null || infos[i].end_time <= 0)
            {
                timers[i] = -1;
                continue;
            }

            var now = DateTime.Now.AddHours(-1); // Hack timezone fix
            var currentSeconds = (now - Constants.Epoch).TotalSeconds;
            var delta = infos[i].end_time - currentSeconds;
            if (delta < 0) delta = 0;
            timers[i] = (float)delta;
        }
    }       

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, 200, 20));
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
            GUILayout.BeginArea(new Rect(20 + (width + 5) * i, 45, width, 400));
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

        if (info.components == null)
        {
            return;
        }

        GUILayout.Space(1);
        GUILayout.Label("<b>Components</b>");
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

        if (info.status != Constants.StatusStrings[StationStatus.Warning])
        {
            return;
        }

        GUILayout.Space(1);
        GUILayout.Label($"<b>Warning * {timers[stationIndex]}</b>");

        if (info.faults == null) return;

        foreach (var faultList in info.faults)
        {
            var otherStationIndex = faultList.station_id;
            var chunks = faultList.chunks;

            var otherStation = infos[otherStationIndex];
            var otherPreset =
                Constants.Presets[infos[otherStationIndex].preset_index];

            GUILayout.Label(otherStation.color);

            if (chunks == null)
            {
                GUILayout.Label("Error, chunk is null");
                continue;
            };

            foreach (var chunk in chunks)
            {
                if (chunk.targets == null)
                {
                    GUILayout.Label("Error, targets is null");
                    continue;
                }

                GUILayout.BeginHorizontal();
                var chunkType = otherPreset[chunk.targets[0].component_id];
                GUILayout.Label(chunkType.ToString());
                foreach (var goal in chunk.targets)
                {
                    GUILayout.Label(goal.target_value.ToString());
                }
                GUILayout.EndHorizontal();
            }
        }
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
            try
            {
                var info = JsonUtility.FromJson<StationInfo>(json);
                infos[stationIndex] = info;
            }
            catch (ArgumentException ae)
            {
                Debug.Log(json);
                Debug.LogException(ae);
                break;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
