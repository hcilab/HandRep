using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T9SummaryLog : BaseDataLog
{

    private float startTime = 0f;
    private float endTime = 0f;

    Dictionary<int, Dictionary<string, float>> data = new Dictionary<int, Dictionary<string, float>>();

    public void Start()
    {
        string headers = this.GetMandatoryHeaders(true);
        headers += ",Start Time,End Time,Task";
        WriteHeader(this.filename, headers, this.filename);
    }

    public void StartTrial(int trial)
    {
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

    public void EndTrial(int trial)
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

        float duration = data[trial]["endTime"] - data[trial]["startTime"];
        string line = "";
        line += data[trial]["startTime"];
        line += "," + data[trial]["endTime"];
        line += "," + this.filename.Replace("_Summary", "");
        WriteSummary(this.filename, line, duration, trial.ToString());
    }

}
