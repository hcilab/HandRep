using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T5SummaryLog : BaseDataLog
{

    private float startTime = 0f;
    private float endTime = 0f;

    private int numTimesChanged = 0;
    private bool usedScale = false;
    private string requiredColour = "none";
    Dictionary<int, Dictionary<string, float>> data = new Dictionary<int, Dictionary<string, float>>();

    public T5_Manager t5m;
    public void Start()
    {
        string headers = this.GetMandatoryHeaders(true);
        headers += ",Start Time,End Time,Task";
        headers += ",Times Changed,Used Scale,First Colour,Required Colour,Final Colour,Match";
        WriteHeader(this.filename, headers, this.filename);
    }

    public void StartTrial(int trial, string colour)
    {
        numTimesChanged = 0;
        requiredColour = colour;
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

    public void EndTrial(int trial, string endColour, string startColour)
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
        line += "," + numTimesChanged.ToString();
        line += "," + usedScale.ToString();
        line += "," + startColour;
        line += "," + requiredColour;
        line += "," + endColour;
        line += "," + (requiredColour.Equals(endColour).ToString());
        
        WriteSummary(this.filename, line, duration, trial.ToString());
        numTimesChanged = 0;
        usedScale = false;
        requiredColour = "none";
    }

    public void IncrementUse()
    {
        numTimesChanged += 1;
    }

    public void ScaleUsage()
    {
        usedScale = true;
    }
}
