using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DialLogger : BaseLogger, IDial
{   //PATTERN
    // handtype, sourcetype, event_type, null,  time, raw_val, value,
    // raw_min, raw_max, min, max

    public string eventType = "";
    [SerializeField] private float min = 0f;
    [SerializeField] private float max = 28f;
    [SerializeField] private float raw_min = 0f;
    [SerializeField] private float raw_max = 360f;

    public bool IsEnabled = true;

    public void DialUpdate(float value)
    {
        string type = "D_dial";
        string eventType = this.eventType;
        float timeMillis = Time.time * 1000;

        float percent = value / raw_max;
        float val = ((max - min) * percent) + min;


        string line = settings.GetCurrentHandModelSet().ToString() + ",";
        line += type + "," + eventType + "," + "null" + "," + timeMillis;
        line += "," + value + "," + val;
        line += "," + raw_min + "," + raw_max;
        line += "," + min + "," + max;

        WriteLog(line, timeMillis);
    }
}
