using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LeverLogger : BaseLogger
{//PATTERN
    // handtype, sourcetype, event_type, pull, time, 
    //event interactor name, event interactor tag, event interactor id,

    public void LogLeverEvent(XRBaseInteractor interactor)
    {
        string type = "D_lever";
        string eventType = "Lever";

        float timeMillis = Time.time * 1000;
        string interactorInfo = "null,null,null";

        interactorInfo = interactor.name + "," + interactor.tag + "," + interactor.GetInstanceID();

        string line = settings.GetCurrentHandModelSet().ToString() + ",";
        line += type + "," + eventType + "," + "pull" + "," + timeMillis;
        line += "," + interactorInfo;
        WriteLog(line, timeMillis);
        // need to put a logger on cauldron, add ingredient, current contents, clear all (and what was in it), bottle dunk, etc.
    }
}
