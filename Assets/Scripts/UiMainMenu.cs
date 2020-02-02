using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiMainMenu : MonoBehaviour
{
    private static bool _muted = true;

    [SerializeField] private GameObject serverOffline;
    [SerializeField] private GameObject computerCountSelect;
    [SerializeField] private GameObject stationSelect;

    [SerializeField] private Image[] stationButtons;

    [SerializeField] private AudioSource backgroundMusic;
    
    private AsyncOperation _currentOperation = null;
    private GameInfo _lastGameInfo;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            // ReSharper disable once AssignmentInConditionalExpression
            if (_muted = !_muted)
            {
                backgroundMusic.Stop();
            }
            else
            {
                backgroundMusic.Play();
            }

            Debug.Log(_muted);
        }


        if (!NetworkManager.InstanceSet) return;

        if (_currentOperation != null)
        {
            if (!_currentOperation.isDone) return;
            _currentOperation = null;
        }

        UpdateUi();

        GetNewInfo();
    }

    private void UpdateUi()
    {
        if (_lastGameInfo == null)
        {
            serverOffline.SetActive(true);
            computerCountSelect.SetActive(false);
            stationSelect.SetActive(false);
            return;
        }

        serverOffline.SetActive(false);

        if (string.IsNullOrEmpty(_lastGameInfo.status))
        {
            Debug.LogError("Failed to get status from server");
            return;
        }

        switch (Constants.StationStatuses[_lastGameInfo.status])
        {
            case StationStatus.Disabled:
            case StationStatus.Stopped:
            case StationStatus.Failed:
                computerCountSelect.SetActive(true);
                stationSelect.SetActive(false);
                return;
        }

        computerCountSelect.SetActive(false);
        stationSelect.SetActive(true);

        for (var i = 0; i < stationButtons.Length; i++)
        {
            if (i < _lastGameInfo.station_count)
            {
                var pallette = Constants.Pallettes[_lastGameInfo.colors[i]];
                stationButtons[i].color = pallette.BackgroundColor;
                stationButtons[i].gameObject.SetActive(true);
            }
            else
            {
                stationButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void GetNewInfo()
    {
        var statusCall = NetworkManager.Instance.Status();
        _currentOperation = statusCall.Item2;

        statusCall.Item2.completed += (_) =>
        {
            if (this == null) return;
            var json = statusCall.Item1.downloadHandler.text;
            _lastGameInfo = JsonUtility.FromJson<GameInfo>(json);
        };
    }


    public void StartGame(int computers)
    {
        NetworkManager.Instance.Setup(computers, -1);
    }

    public void JoinGame(int computer)
    {
        StationManager.StationId = computer;
        _currentOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
}
