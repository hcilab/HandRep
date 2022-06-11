using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CmdLineManager : MonoBehaviour
{
    public string participantID { get; set; } = System.DateTime.Now.ToString("yyyyMMddHHmmss");
    public bool isSeated = true;
    private static string GetArg(string name)
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        string seatArg = GetArg("-seated");
        if (seatArg != null && !seatArg.Equals(null) && seatArg.Equals("true"))
        {
            isSeated = true;
        }
        else if (seatArg != null && !seatArg.Equals(null) && seatArg.Equals("false"))
        {
            isSeated = false;
        }

        string idArg = GetArg("-pid");
        if (idArg != null && !idArg.Equals(null))
        {
            participantID = idArg;
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

    public string GetPID()
    {
        return participantID;
    }

}
