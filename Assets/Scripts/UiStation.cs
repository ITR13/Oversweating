using System;
using UnityEngine;
using UnityEngine.UI;

public class UiStation : MonoBehaviour
{
    public int presetId = -1;
    [SerializeField]
    private Image componentBackground, alarmBackground;

    [SerializeField] private UiComponent[] uiComponents = new UiComponent[0];

    public void UpdateInfo(StationInfo stationInfo, Action<int, int> onClick)
    {
        var pallette = Constants.Pallettes[stationInfo.color];
        // componentBackground.color = pallette.BackgroundColor;
        // alarmBackground.color = pallette.BackgroundColor;

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
    }
}
