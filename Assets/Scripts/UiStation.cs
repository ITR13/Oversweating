using System;
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

        if (!warn || stationInfo.fault_count == prevFaultCount) return;
        prevFaultCount = stationInfo.fault_count;


        foreach (var faultList in stationInfo.faults)
        {
            AddTask(faultList);
        }
    }

    private void AddTask(FaultList faultList)
    {
        var child = Instantiate(taskPrefab, taskParent);
        child.Set(faultList);
    }
}
