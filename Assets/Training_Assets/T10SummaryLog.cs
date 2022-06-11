using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T10SummaryLog : BaseDataLog
{

    private float startTime = 0f;
    private float endTime = 0f;

    Dictionary<int, Dictionary<string, float>> data = new Dictionary<int, Dictionary<string, float>>();

    public int numTimesPoured = 0;
    public bool exceeded = false;

    public void Start()
    {
        string headers = this.GetMandatoryHeaders(true);
        headers += ",Start Time,End Time,Task,Range Min,Range Max,Value,Exceeded,Times Poured";
        WriteHeader(this.filename, headers, this.filename);
    }

    public void StartTrial(int trial)
    {
        numTimesPoured = 0;
        exceeded = false; ;
        float timeMillis = Time.time * 1000;
        if (data.ContainsKey(trial))
        {
            data[trial]["startTime"] = timeMillis;
        }
        else
        {
            data.Add(trial, new Dictionary<string, float>());
            data[trial].Add("startTime", timeMillis);
        }
    }

    public void EndTrial(int trial, float min, float max, float value)
    {
        float timeMillis = Time.time * 1000;
        if (data[trial].ContainsKey("endTime"))
        {
            data[trial]["endTime"] = timeMillis;
        }
        else
        {
            data[trial].Add("endTime", timeMillis);
        }

        if (exceeded)
        {
            value = max;
        }

        float duration = data[trial]["endTime"] - data[trial]["startTime"];
        string line = "";
        line += data[trial]["startTime"];
        line += "," + data[trial]["endTime"];
        line += "," + this.filename.Replace("_Summary", "");
        line += "," + min.ToString("f2");
        line += "," + max.ToString("f2");
        line += "," + value.ToString("f2");
        line += "," + exceeded.ToString();
        line += "," + numTimesPoured.ToString();

        WriteSummary(this.filename, line, duration, trial.ToString());
        numTimesPoured = 0;
        exceeded = false;
    }

}
