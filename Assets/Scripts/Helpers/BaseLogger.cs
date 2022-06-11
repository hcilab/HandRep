using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class BaseLogger : MonoBehaviour
{
    protected DataManager dm => FindObjectOfType<DataManager>();
    protected Settings settings => FindObjectOfType<Settings>();
    protected void WriteLog(string data, float time)
    {
        if (dm != null)
        {
            dm.LogLine(data, time);
        }
    }
}
