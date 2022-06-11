using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BaseDataLog : MonoBehaviour
{

    public enum FileNameAppending
    {
        None,
        HandType,
        Tier,
        HandTypeAndTier
    }
    // /DATA/<pid>/<anything such as nothing, Tiers, Sphere/Index/Hand, Training, and sub directories>
    private Settings settings => FindObjectOfType<Settings>();
    private string dataDirectory => Application.persistentDataPath + "/DATA";
    [ReadOnly, SerializeField] private string participantID;
    [SerializeField] protected string directory;
    [SerializeField] protected string filename;
    [SerializeField] protected string fileExt = ".csv";
    //[SerializeField] public FileNameAppending AppendingType = FileNameAppending.HandTypeAndTier;

    [ReadOnly, SerializeField] private string fullDirectory;
    //private string dataFile;
    private void Start()
    {
        participantID = settings.GetPID();
        if (!fileExt.StartsWith("."))
        {
            fileExt = "." + fileExt;
        }
        string dataDirectory = Application.persistentDataPath + "/DATA";
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }
        dataDirectory += "/" + participantID;
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }
        if (!Directory.Exists(dataDirectory + "/Game"))
        {
            Directory.CreateDirectory(dataDirectory + "/Game");
        }
        if (!Directory.Exists(dataDirectory + "/Training"))
        {
            Directory.CreateDirectory(dataDirectory + "/Training");
        }
    }

    public string GetMandatoryColumns(string task = "null")
    {
        string line = "";
        float timeMillis = Time.time * 1000;
        line += settings.GetPID();
        line += "," + timeMillis.ToString();
        line += "," + settings.GetCurrentHandModelSet().ToString();
        string order = "";
        order += settings.order[0].ToString() + "|";
        order += settings.order[1].ToString() + "|";
        order += settings.order[2].ToString();
        line += "," + order;
        line += "," + settings.CurrentTier;
        line += "," + task;
        return line;
    }

    public string GetMandatorySummaryColumns(float duration, string trial = "null")
    {
        string line = "";
        line += settings.GetPID();
        line += "," + duration.ToString();
        line += "," + settings.GetCurrentHandModelSet().ToString();
        string order = "";
        order += settings.order[0].ToString() + "|";
        order += settings.order[1].ToString() + "|";
        order += settings.order[2].ToString();
        line += "," + order;
        line += "," + settings.CurrentTier;
        line += "," + trial;
        return line;
    }

    public string GetMandatoryHeaders(bool isSummary = false)
    {
        string line = "";
        line += "Participant";
        line += "," + (isSummary ? "Duration MS" : "Time");
        line += "," + "HandModel";
        line += "," + "Order";
        line += "," + "Tier";
        line += "," + (isSummary ? "Trial" : "Task");
        return line;
    }

    /// <summary>
    /// Filename: The filename without path or final extension. I.E. Interactable.Hand
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="line"></param>
    public void Write(string filename, string data, string task = "null")
    {
        fullDirectory = dataDirectory + "/" + settings.GetPID() + "/" + directory;
        float timeMillis = Time.time * 1000;
        if (timeMillis <= 100f)
        {
            return;
        }
        float sinceLoad = Time.timeSinceLevelLoad;
        if (sinceLoad <= 5f)
        {
            return;
        }
        
        string file = fullDirectory + "/" + filename + fileExt;
        if (!File.Exists(file))
        {
            return;
        }
        data = GetMandatoryColumns(task) + "," + data;
        StreamWriter writer = new StreamWriter(file, true);
        writer.WriteLine(data);
        writer.Flush();
        writer.Close();
    }

    public void WriteSummary(string filename, string data, float duration, 
        string trial = "-1")
    {
        fullDirectory = dataDirectory + "/" + settings.GetPID() + "/" + directory;
        float timeMillis = Time.time * 1000;
        if (timeMillis <= 100f)
        {
            return;
        }
        float sinceLoad = Time.timeSinceLevelLoad;
        if (sinceLoad <= 5f)
        {
            return;
        }
        
        string file = fullDirectory + "/" + filename + fileExt;
        if (!File.Exists(file))
        {
            return;
        }
        data = GetMandatorySummaryColumns(duration, trial) + (data.Equals("") ? "" : "," + data);
        StreamWriter writer = new StreamWriter(file, true);
        writer.WriteLine(data);
        writer.Flush();
        writer.Close();

    }

    public void WriteHeader(string filename, string data, string task = "null")
    {
        fullDirectory = dataDirectory + "/" + settings.GetPID() + "/" + directory;
        string file = fullDirectory + "/" + filename + fileExt;
        if (!File.Exists(file))
        {
            File.WriteAllText(file, "");
        }
        else
        {
            return;
        }
        StreamWriter writer = new StreamWriter(file, true);
        writer.WriteLine(data);
        writer.Flush();
        writer.Close();
        //Debug.Log(file);
    }

    public virtual void LogEvent(BaseInteractionEventArgs args)
    { 
    }
}
