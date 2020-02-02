using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class StationInfo
{
    public string color;
    public int[] components;
    public int preset_index;
    public string status;
    public FaultList[] faults;
    public int fault_count;
    public double end_time;

    public override string ToString()
    {
        return $"[{color}] Status is {status}.";
    }
}

[Serializable]
public class FaultList
{
    public int station_id;
    public TargetChunk[] chunks;
}

[Serializable]
public class TargetChunk
{
    public string component_name;
    public Target[] targets;
}

[Serializable]
public class Target
{
    public int component_id;
    public int target_value;
}