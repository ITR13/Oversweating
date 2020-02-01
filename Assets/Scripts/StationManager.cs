using System;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

public class StationManager : MonoBehaviour
{
    public static int StationId = -1;

    private StationInfo _myStationInfo;
    
    private UiStation _activeUiStation = null;
    private AsyncOperation _currentOperation = null;

    private void OnEnable()
    {
        UiStation foundUiStation = null;

        var sceneCount = SceneManager.sceneCount;
        var myScene = gameObject.scene;
        for (var sceneIndex = 0; sceneIndex < sceneCount; sceneIndex++)
        {
            var scene = SceneManager.GetSceneAt(sceneIndex);
            if (scene == myScene)
            {
                continue;
            }

            var roots = scene.GetRootGameObjects();
            if (roots.Length != 1)
            {
                Debug.LogError("Incorrect scene count");
                Exit();
            }

            var uiStation = roots[0].GetComponent<UiStation>();
            if (uiStation == null)
            {
                Debug.LogError(
                    "Preset scene was missing UiStation component on root"
                );
                Exit();
            }else if (foundUiStation != null)
            {
                Debug.LogError("Multiple Ui Stations");
                Exit();
            }
            else if (uiStation.presetId == -1)
            {
                Debug.LogError("Preset Id was -1");
                Exit();
            }

            foundUiStation = uiStation;
        }

        _activeUiStation = foundUiStation;
    }

    private void Update()
    {
        if (!NetworkManager.InstanceSet) return;

        if (_currentOperation != null)
        {
            if (!_currentOperation.isDone) return;
            _currentOperation = null;
        }

        if (StationId == -1)
        {
            if (_activeUiStation != null)
            {
                _currentOperation = SceneManager.UnloadSceneAsync(_activeUiStation.gameObject.scene);
            }
            return;
        }
        
        if (_myStationInfo == null)
        {
            UpdateStationInfo();
            return;
        }
        
        if (_activeUiStation == null)
        {
            _currentOperation = SceneManager.LoadSceneAsync(_myStationInfo.preset_index + 2, LoadSceneMode.Additive);
            if (_currentOperation == null)
            {
                Exit();
                return;
            }
            _currentOperation.completed += (_) =>
            {
                var loadedScene =
                    SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
                if (loadedScene.rootCount != 1)
                {
                    Debug.LogError("Invalid rootcount in loaded scene");
                    Exit();
                }

                var root = loadedScene.GetRootGameObjects()[0];
                _activeUiStation = root.GetComponent<UiStation>();
                if (_activeUiStation == null)
                {
                    Debug.LogError("Root object was missing UiStation");
                    Exit();
                    return;
                }

                if (_activeUiStation.presetId != _myStationInfo.preset_index)
                {
                    Debug.LogError($"Loaded preset doesn't match with preset index: {_activeUiStation.presetId} != {_myStationInfo.preset_index}");
                    Exit();
                    return;
                }

                _activeUiStation.UpdateInfo(_myStationInfo);

            };
            return;
        }
        
        if(_activeUiStation.presetId != _myStationInfo.preset_index)
        {
            _currentOperation =
                SceneManager.UnloadSceneAsync(
                    _activeUiStation.gameObject.scene
                );
            return;
        }

        UpdateStationInfo();
    }

    private void UpdateStationInfo()
    {
        var (request, op) = NetworkManager.Instance.Info(StationId);
        _currentOperation = op;
        op.completed += (_) =>
        {
            if (request.downloadProgress < 1)
            {
                Debug.LogError("Op finished, but download didn't");
                return;
            }

            try
            {
                _myStationInfo =
                    JsonUtility.FromJson<StationInfo>(
                        request.downloadHandler.text
                    );
            }
            catch (ArgumentException ae)
            {
                Debug.Log(request.downloadHandler.text);
                Debug.LogException(ae);
                return;
            }

            if (_activeUiStation != null)
            {
                _activeUiStation.UpdateInfo(_myStationInfo);
            }
        };
    }

    private void _currentOperation_completed(AsyncOperation obj)
    {
        throw new System.NotImplementedException();
    }

    private static void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
