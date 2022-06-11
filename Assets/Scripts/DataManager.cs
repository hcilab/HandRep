using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    private List<string> lineBuffer = new List<string>();
    private string fileDirectory => Application.persistentDataPath + "/DATA";
    private string fileExtension = ".txt";
    [ReadOnly, SerializeField] private int userCount = 0;
    [ReadOnly, SerializeField] private string dataFile = "";
    private Settings settings => FindObjectOfType<Settings>();

    [SerializeField] bool EnableConsoleLogging = true;
    [ReadOnly, SerializeField] private int lineCount = 0;

    public bool Logging = true;
    public bool DebugLogging = true;
    private void Awake()
    {
        if(!Directory.Exists(fileDirectory))
        {
            Directory.CreateDirectory(fileDirectory);
        }
        DirectoryInfo di = new DirectoryInfo(fileDirectory);
        FileInfo[] fis = di.GetFiles();
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Equals(fileExtension, System.StringComparison.OrdinalIgnoreCase))
            {
                userCount++;
            }
        }
        dataFile = fileDirectory + "/" + userCount.ToString() + fileExtension;
        if (!File.Exists(dataFile))
        {
            File.WriteAllText(dataFile, "");
        }
    }
    public void LogLine(string line, float time)
    {
        if(!Logging)
        {
            return;
        }
        if (dataFile.Equals("") || time == 0f)
        {
            return;
        }

        if (lineBuffer.Count >= 25)
        {
            StreamWriter writer = new StreamWriter(dataFile, true);
            //File.AppendAllLines(dataFile, lineBuffer);
            string contents = string.Join("\n", lineBuffer);
            lineBuffer.Clear();
            writer.WriteLine(contents);
            writer.Flush();
            writer.Close();
        }
        lineBuffer.Add(line);
        lineCount += 1;
        if(DebugLogging)
            Debug.Log("<color=blue>" + line + "</color>");
    
    }

    public void ForceLog()
    {
        if(!Logging)
        {
            return;
        }
        StreamWriter writer = new StreamWriter(dataFile, true);
        //File.AppendAllLines(dataFile, lineBuffer);
        string contents = string.Join("\n", lineBuffer);
        lineBuffer.Clear();
        writer.WriteLine(contents);
        writer.Flush();
        writer.Close();
    }

    private void OnDestroy()
    {
        ForceLog();
        if(DebugLogging)
            Debug.Log("<color=blue>" + lineCount + "</color>");
    }

}
