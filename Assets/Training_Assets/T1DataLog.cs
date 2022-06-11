using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T1DataLog : BaseDataLog
{
    private enum State
    {
        None,
        PickedUp,
        LetGo,
        Placed,
        Removed,
        RemovedFromSocket,
        ItemTimeout
    }

    public void Start()
    {
        string headers = this.GetMandatoryHeaders();
        headers += ",State";
        headers += ",Object ID";
        headers += ",Object Name";
        headers += ",Source Selector";
        headers += ",Source Selector ID";
        headers += ",Object Weight";
        WriteHeader(this.filename, headers, this.filename);
    }

    public void LogGrabEvent(SelectEnterEventArgs args)
    {
        State state = State.PickedUp;
        if (!(args.interactor is XRDirectInteractor))
        {
            return;
        }
        string instanceID = args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        string tag = (args.interactor != null ? args.interactor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + tag;
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        string w = props.GetProperty("weight");
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");
        line += "," + weight;
        Write(this.filename, line, this.filename);
    }
    public void LogRemovedFromSocketEvent(SelectExitEventArgs args)
    {
        State state = State.RemovedFromSocket;
        string instanceID = args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        line += "," + (args.interactor != null ? args.interactor.tag : "none");
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        line += "," + "none";
        Write(this.filename, line, this.filename);
    }

    public void LogLetGoEvent(SelectExitEventArgs args)
    {
        State state = State.LetGo;
        if (!(args.interactor is XRDirectInteractor))
        {
            return;
        }
        string instanceID = args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        string tag = (args.interactor != null ? args.interactor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + tag;
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        string w = props.GetProperty("weight");
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");
        line += "," + weight;
        Write(this.filename, line, this.filename);
    }

    public void LogPlacedEvent(SelectEnterEventArgs args)
    {
        State state = State.Placed;
        string instanceID = args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        line += "," + (args.interactor != null ? args.interactor.tag : "none");
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        string w = props.GetProperty("weight");
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");
        line += "," + weight;
        Write(this.filename, line, this.filename);
    }
    public void LogRemovedEvent(SelectExitEventArgs args)
    {
        State state = State.Removed;
        string instanceID = args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        line += "," + (args.interactor != null ? args.interactor.tag : "none");
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        string w = props.GetProperty("weight");
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");
        line += "," + weight;
        Write(this.filename, line, this.filename);
    }
}
