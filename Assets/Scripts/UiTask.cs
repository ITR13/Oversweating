using System;
using UnityEngine;
using UnityEngine.UI;

public class UiTask : MonoBehaviour
{
    [SerializeField]
    private Image stationIndicator;

    [SerializeField]
    private Transform iconParent;

    [SerializeField]
    private Image iconPrefab;

    public void Set(FaultList faultList)
    {
        stationIndicator.sprite =
            Constants.InstructionIcons[Tuple.Create(
                "station",
                faultList.station_id
            )];


        foreach (var chunk in faultList.chunks)
        {
            var componentName = chunk.component_name;
            foreach (var target in chunk.targets)
            {
                var sprite = Constants.InstructionIcons[
                    Tuple.Create(
                        componentName,
                        target.target_value
                    )
                ];
                AddIcon(sprite);
            }
        }
    }

    private void AddIcon(Sprite sprite)
    {
        var child = Instantiate(iconPrefab, iconParent);
        child.sprite = sprite;
    }
}
