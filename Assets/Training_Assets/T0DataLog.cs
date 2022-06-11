using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T0DataLog : BaseDataLog
{
    private enum State
    {
        None,
        PickedUp,
        LetGo,
        ValidDrop,
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
        WriteHeader(this.filename, headers, this.filename);
    }

    public void LogGrabEvent(SelectEnterEventArgs args)
    {
        State state = State.PickedUp;
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
        Write(this.filename, line, this.filename);
    }

    public void LogLetGoEvent(SelectExitEventArgs args)
    {
        State state = State.LetGo;
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
        Write(this.filename, line, this.filename);
    }

    public void LogValidDropEvent(HoverEnterEventArgs args)
    {
        State state = State.ValidDrop;
        string instanceID = args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        line += "," + (args.interactor != null ? args.interactor.name : "none");
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        Write(this.filename, line, this.filename);
    }
}
