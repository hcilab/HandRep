using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class ButtonLogger : BaseLogger
{//PATTERN
    // handtype, sourcetype, event_type, press, time, 
    //event interactor name, event interactor tag, event interactor id,

    public void LogButtonEvent(XRBaseInteractor interactor)
    {
        string type = "D_button";
        string eventType = "Button";

        float timeMillis = Time.time * 1000;
        string interactorInfo = "null,null,null";

        interactorInfo = interactor.name + "," + interactor.tag + "," + interactor.GetInstanceID();
        
        string line = settings.GetCurrentHandModelSet().ToString() + ",";
        line += type + "," + eventType + "," + "press" + "," + timeMillis;
        line += "," + interactorInfo;
        WriteLog(line, timeMillis);
    }
}
