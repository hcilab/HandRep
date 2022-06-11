using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T7DataLog : BaseDataLog
{
    private enum State
    {
        None,
        PickedUp,
        Dropped,
        GrindRotation,
        Grinded,
        RemovedFromSocket,
        Grabbed,
        LetGo,
        AddedToBowl,
        RemovedFromBowl,
        ItemTimeout
    }

    private void Awake()
    {
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
        if (!(args.interactor is XRDirectInteractor))
        {
            state = State.AddedToBowl;
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

    public void LogDroppedEvent(SelectExitEventArgs args)
    {
        State state = State.Dropped;
        if (!(args.interactor is XRDirectInteractor))
        {
            state = State.RemovedFromBowl;
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
        Write(this.filename, line, this.filename);
    }

    public void LogHandleEvent(BaseInteractionEventArgs args)
    {
        if (!(args is SelectEnterEventArgs || args is SelectExitEventArgs))
        {
            return;
        }
        State state = args is SelectEnterEventArgs ? State.Grabbed : State.LetGo;
        if (!(args.interactor is XRDirectInteractor))
        {
            return;
        }
        string instanceID = args.interactable.GetInstanceID().ToString();

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + "handle";
        string tag = (args.interactor != null ? args.interactor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + tag;
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        Write(this.filename, line, this.filename);
    }

    public void LogGrindEvent()
    {
        State state = State.Grinded;
        string line = "";
        line += state.ToString();
        line += "," + "none";
        line += "," + "none";
        string tag = "none";
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + tag;
        line += "," + "none";
        Write(this.filename, line, this.filename);
    }
    public void LogRotationEvent()
    {
        State state = State.GrindRotation;
        string line = "";
        line += state.ToString();
        line += "," + "none";
        line += "," + "none";
        string tag = "none";
        line += "," + tag;
        line += "," + "none";
        Write(this.filename, line, this.filename);
    }

}
