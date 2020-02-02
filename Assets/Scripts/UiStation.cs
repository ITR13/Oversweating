﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiStation : MonoBehaviour
{
    public int presetId = -1;
    [SerializeField]
    private Image[] backgrounds;

    [SerializeField] private UiComponent warningSign = null;
    [SerializeField] private UiComponent[] uiComponents = new UiComponent[0];

    [SerializeField] private Transform taskParent;
    [SerializeField] private UiTask taskPrefab;

    private int prevFaultCount = -1;
    private List<Transform> spawnedTasks = new List<Transform>();

    [SerializeField]
    private AudioSource alarm;

    private float _targetVolume = 0;

    public void UpdateInfo(StationInfo stationInfo, Action<int, int> onClick)
    {
        var pallette = Constants.Pallettes[stationInfo.color];
        /*
        foreach (var background in backgrounds)
        {
            background.color = pallette.BackgroundColor;
        }
        */

        var warn = stationInfo.status ==
                   Constants.StatusStrings[StationStatus.Warning];

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
            DeleteTasks();
            return;
        }

        if (stationInfo.fault_count == prevFaultCount) return;
        prevFaultCount = stationInfo.fault_count;

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
