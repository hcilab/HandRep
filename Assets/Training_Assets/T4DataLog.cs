using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T4DataLog : BaseDataLog
{
    private enum State
    {
        None,
        PickedUp,
        Dropped,
        Grabbed,
        LetGo,
        Opened,
        Closed,
        ItemAdded
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

    public void LogChestGrabEvent(SelectEnterEventArgs args)
    {
        State state = State.Grabbed;

        string line = "";
        line += state.ToString();
        string tag = (args.interactor != null ? args.interactor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + args.interactable.GetInstanceID();
        line += "," + args.interactable.gameObject.tag;
        line += "," + tag;
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        Write(this.filename, line, this.filename);
    }

    public void LogChestLetGoEvent(SelectExitEventArgs args)
    {
        State state = State.LetGo;
        string line = "";
        line += state.ToString();
        string tag = (args.interactor != null ? args.interactor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + args.interactable.GetInstanceID().ToString();
        line += "," + args.interactable.gameObject.tag;
        line += "," + tag;
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        Write(this.filename, line, this.filename);
    }

    public void LogPickupEvent(SelectEnterEventArgs args)
    {
        State state = State.PickedUp;

        string line = "";
        line += state.ToString();
        string tag = (args.interactor != null ? args.interactor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");
        line += "," + cleanName;
        line += "," + tag;
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        Write(this.filename, line, this.filename);
    }

    public void LogDroppedEvent(SelectExitEventArgs args)
    {
        State state = State.Dropped;
        string line = "";
        line += state.ToString();
        string tag = (args.interactor != null ? args.interactor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");
        line += "," + cleanName;
        line += "," + tag;
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        Write(this.filename, line, this.filename);
    }

    public void LogItemAddedEvent(HoverEnterEventArgs args)
    {
        State state = State.ItemAdded;
        string line = "";
        line += state.ToString();
        string tag = (args.interactor != null ? args.interactor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");
        line += "," + cleanName;
        line += "," + "ChestBase";
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        Write(this.filename, line, this.filename);
    }

    public void LogChestOpen()
    {
        State state = State.Opened;
        string line = "";
        line += state.ToString();

        line += "," + "none";
        line += "," + "none";
        line += "," + "none";
        line += "," + "none";
        Write(this.filename, line, this.filename);
    }

    public void LogChestClosed()
    {
        State state = State.Closed;
        string line = "";
        line += state.ToString();

        line += "," + "none";
        line += "," + "none";
        line += "," + "none";
        line += "," + "none";
        Write(this.filename, line, this.filename);
    }
}
