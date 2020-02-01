using UnityEngine;
using UnityEngine.UI;

public class UiStation : MonoBehaviour
{
    public int presetId = -1;
    [SerializeField]
    private Image componentBackground, alarmBackground;

    [SerializeField] private UiComponent[] uiComponents = new UiComponent[0];

    public void UpdateInfo(StationInfo stationInfo)
    {
        var pallette = Constants.Pallettes[stationInfo.color];
        // componentBackground.color = pallette.BackgroundColor;
        // alarmBackground.color = pallette.BackgroundColor;

        for (var i = 0; i < uiComponents.Length; i++)
        {
            uiComponents[i].color = pallette.Index;
            uiComponents[i].state =
                i < stationInfo.components.Length
                    ? stationInfo.components[i]
                    : 0;
        }
    }
}
