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
    public Tuple<int, int[][]>[] faults;
    public int preset_index;
    public string status;

    public override string ToString()
    {
        return $"[{color}] Status is {status}.";
    }
}
