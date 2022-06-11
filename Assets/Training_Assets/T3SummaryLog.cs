using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T3SummaryLog : BaseDataLog
{

    private float startTime = 0f;
    private float endTime = 0f;
    public int numTimesGrabbed = 0;

    Dictionary<int, Dictionary<string, float>> data = new Dictionary<int, Dictionary<string, float>>();
    public void Start()
    {
        string headers = this.GetMandatoryHeaders(true);
        headers += ",Start Time,End Time,Task";
        headers += ",Times Grabbed,Min,Max,Value,In Range";
        WriteHeader(this.filename, headers, this.filename);
    }

    public void StartTrial(int trial)
    {
        numTimesGrabbed = 0;
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

    public void Grabbed(SelectEnterEventArgs args)
    {
        numTimesGrabbed += 1;
    }

    public void EndTrial(int trial, int min, int max, int value)
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
        line += "," + numTimesGrabbed;
        line += "," + min.ToString();
        line += "," + max.ToString();
        line += "," + value.ToString();
        line += "," + (value >= min && value <= max ? "True" : "False");
        WriteSummary(this.filename, line, duration, trial.ToString());
        numTimesGrabbed = 0;
    }
}
