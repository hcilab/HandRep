using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TaskSummaryLog : BaseDataLog
{

    private float startTime = 0f;
    private float endTime = 0f;

    public int timesReset = 0;
    public int timesScooped = 0;
    public int timesEnchanted = 0;
    public int timesGrinded = 0;
    public int timesWeighed = 0;
    public int totalAdded = 0;
    public int totalLiquidsAdded = 0;
    public int timesCubesChanged = 0;
    public int timesGrabbedValve = 0;

    Dictionary<int, Dictionary<string, float>> data = new Dictionary<int, Dictionary<string, float>>();

    public void Start()
    {
        string headers = this.GetMandatoryHeaders(true);
        headers += ",Start Time,End Time";
        headers += ",Times Cleared,Times Scooped,Times Enchanted,Times Grinded";
        headers += ",Times Weighed,Total Items Added,Total Liquids Added";
        headers += ",Times Cubes Changed,Times Grabbed Valve,Result";
        WriteHeader(this.filename, headers, this.filename);
    }

    public void StartTrial(int task)
    {
        float timeMillis = Time.time * 1000;
        if (data.ContainsKey(task))
        {
            data[task]["startTime"] = timeMillis;
        }
        else
        {
            data.Add(task, new Dictionary<string, float>());
            data[task].Add("startTime", timeMillis);
        }
        this.ResetVals();
    }

    public void EndTrial(int task, bool match)
    {
        float timeMillis = Time.time * 1000;
        if (data[task].ContainsKey("endTime"))
        {
            data[task]["endTime"] = timeMillis;
        }
        else
        {
            data[task].Add("endTime", timeMillis);
        }

        float duration = data[task]["endTime"] - data[task]["startTime"];
        string line = "";
        line += data[task]["startTime"];
        line += "," + data[task]["endTime"];
        line += "," + timesReset;
        line += "," + timesScooped;
        line += "," + timesEnchanted;
        line += "," + timesGrinded;
        line += "," + timesWeighed;
        line += "," + totalAdded;
        line += "," + totalLiquidsAdded;
        line += "," + timesCubesChanged;
        line += "," + timesGrabbedValve;
        line += "," + (match ? "Success" : "Fail");

        WriteSummary(this.filename, line, duration, task.ToString());
        this.ResetVals();
    }

    public void ResetVals()
    {
        timesReset = 0;
        timesScooped = 0;
        timesEnchanted = 0;
        timesGrinded = 0;
        timesWeighed = 0;
        totalAdded = 0;
        totalLiquidsAdded = 0;
        timesCubesChanged = 0;
        timesGrabbedValve = 0;
    }

    public void IncrementValveGrabs()
    {
        timesGrabbedValve += 1;
    }

    public void IncrementClears()
    {
        timesReset += 1;
    }

    public void IncrementScoop()
    {
        timesScooped += 1;
    }

    public void IncrementEnchants()
    {
        timesEnchanted += 1;
    }

    public void IncrementWeighs()
    {
        timesWeighed += 1;
    }

    public void IncrementAdded()
    {
        totalAdded += 1;
    }

    public void IncrementLiquids()
    {
        // not ml, but will be for each particle that was added. so each one represents 0.10
        totalLiquidsAdded += 1;
    }

    public void IncrementCubeChange()
    {
        timesCubesChanged += 1;
    }

    public void IncrementGrinds()
    {
        timesGrinded += 1;
    }
}
