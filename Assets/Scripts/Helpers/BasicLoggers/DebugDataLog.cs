using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDataLog : BaseDataLog
{
    public void Start()
    {
        string headers = this.GetMandatoryHeaders();
        WriteHeader(this.filename, headers, this.filename);
    }
    public void LogEvent()
    {
        
        string line = "DEBUG ---";

        Write(this.filename, line, this.filename);
    }
}
