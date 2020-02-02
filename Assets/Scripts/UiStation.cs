using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiStation : MonoBehaviour
{
    public int presetId = -1;
    [SerializeField]
    private Image[] backgrounds;

    [SerializeField] private PopupScript popupScript;

    [SerializeField] private UiComponent warningSign = null;
    [SerializeField] private UiComponent[] uiComponents = new UiComponent[0];

    [SerializeField] private Transform taskParent;
    [SerializeField] private UiTask taskPrefab;

    private int prevFaultÌd = -1;
    private List<Transform> spawnedTasks = new List<Transform>();

    [SerializeField]
    private AudioSource alarm;

    [SerializeField]
    private Image healthBar;

    private float _targetVolume = 0;

    public void UpdateInfo(StationInfo stationInfo, Action<int, int> onClick, Action ready)
    {
        if (stationInfo == null)
        {
            popupScript.Open(
                "SERVER OFFLINE",
                Constants.Gray,
                Color.white,
                () => SceneManager.LoadScene(0)
            );
            return;
        }

        healthBar.fillAmount = (float)stationInfo.health;

        var warn = false;
        _targetVolume = 0;
        switch (Constants.StationStatuses[stationInfo.status])
        {
            case StationStatus.Stopped:
            case StationStatus.Disabled:
                return;
            case StationStatus.Warning:
                warn = true;
                break;
            case StationStatus.Failed:
                popupScript.Open(
                    "FAILURE",
                    Constants.Red,
                    Constants.Black,
                    () => SceneManager.LoadScene(0)
                );
                return;
            case StationStatus.Waiting:
                popupScript.Open(
                    "WAITING",
                    Constants.Gray,
                    Color.white,
                    ready
                );
                return;
            case StationStatus.Ready:
                popupScript.Open("READY", Constants.Green, Constants.Black);
                return;
        }

        popupScript.Close();

        var pallette = Constants.Pallettes[stationInfo.color];

        /*
        if (warn)
        {
            foreach (var background in backgrounds)
            {
                background.color = pallette.BackgroundColor;
            }
        }
        */

        _targetVolume = warn ? 1 : 0;

        warningSign.color = pallette.Index;
        warningSign.state = warn ? 1 : 0;

        for (var i = 0; i < uiComponents.Length; i++)
        {
            var componentIndex = i;
            uiComponents[i].color = pallette.Index;
            uiComponents[i].state =
                i < stationInfo.components.Length
                    ? stationInfo.components[i]
                    : 0;

            if(onClick!=null)
            {
                uiComponents[i].onClick = (button) =>
                    onClick.Invoke(componentIndex, button);
            }
        }

        if (!warn)
        {
            prevFaultÌd = stationInfo.fault_id;
            DeleteTasks();
            return;
        }

        if (stationInfo.fault_id == prevFaultÌd) return;
        prevFaultÌd = stationInfo.fault_id;

        DeleteTasks();
        foreach (var faultList in stationInfo.faults)
        {
            AddTask(faultList);
        }
    }

    private void AddTask(FaultList faultList)
    {
        var child = Instantiate(taskPrefab, taskParent);
        child.Set(faultList);
        spawnedTasks.Add(child.transform);
    }

    private void DeleteTasks()
    {
        foreach (var task in spawnedTasks)
        {
            Destroy(task.gameObject);
        }

        spawnedTasks.Clear();
    }


    private void Update()
    {
        if (Math.Abs(alarm.volume - _targetVolume) > 0.01)
        {
            if (alarm.volume < 0.01)
            {
                alarm.Play();
            }
        }
        else
        {
            return;
        }

        alarm.volume = Mathf.MoveTowards(
            alarm.volume,
            _targetVolume,
            Time.deltaTime * 10
        );


        if (_targetVolume < 0.1 && Math.Abs(alarm.volume - _targetVolume) < 0.01)
        {
            alarm.Stop();
        }
    }
}
