using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T5DataLog : BaseDataLog
{

    [SerializeField] T5_Manager t5m;// = FindObjectOfType<T5_Manager>();
    private enum State
    {
        None,
        PickedUp,
        LetGo,
        ValidDrop,
        RemovedFromSocket,
        ColourChange,
        PlacedOnScale,
        RemovedFromScale,
        ItemTimeout
    }

    private void Awake()
    {
        if(t5m == null)
        {
            t5m = FindObjectOfType<T5_Manager>();
        }
    }

    public void Start()
    {
        string headers = this.GetMandatoryHeaders();
        headers += ",State";
        headers += ",Object ID";
        headers += ",Object Name";
        headers += ",Source Selector";
        headers += ",Source Selector ID";
        headers += ",Side Dropped";
        headers += ",Requested Colour";
        headers += ",Colour";
        headers += ",Side";
        headers += ",Result";
        WriteHeader(this.filename, headers, this.filename);
    }

    public void LogGrabEvent(SelectEnterEventArgs args)
    {
        State state = State.PickedUp;
        if(!(args.interactor is XRDirectInteractor))
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
        line += "," + "none";
        line += "," + t5m.colour;
        line += "," + args.interactable.GetComponent<CubeColorController>().GetColour().ToString();
        line += "," + args.interactable.GetComponent<CubeColorController>().GetIndex().ToString();
        line += ",none";
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
        line += "," + t5m.colour;
        line += "," + args.interactable.GetComponent<CubeColorController>().GetColour().ToString();
        line += "," + args.interactable.GetComponent<CubeColorController>().GetIndex().ToString();
        line += ",none";
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
        line += "," + "none";
        line += "," + t5m.colour;
        line += "," + args.interactable.GetComponent<CubeColorController>().GetColour().ToString();
        line += "," + args.interactable.GetComponent<CubeColorController>().GetIndex().ToString();
        line += ",none";
        Write(this.filename, line, this.filename);
    }

    public void LogRemovedEvent(SelectExitEventArgs args)
    {
        State state = State.RemovedFromScale;
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
        line += "," + "none";
        line += "," + t5m.colour;
        line += "," + args.interactable.GetComponent<CubeColorController>().GetColour().ToString();
        line += "," + args.interactable.GetComponent<CubeColorController>().GetIndex().ToString();
        line += ",none";
        Write(this.filename, line, this.filename);
    }

    public void LogPlacedEvent(SelectEnterEventArgs args)
    {
        State state = State.PlacedOnScale;
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
        line += "," + "none";
        line += "," + t5m.colour;
        line += "," + args.interactable.GetComponent<CubeColorController>().GetColour().ToString();
        line += "," + args.interactable.GetComponent<CubeColorController>().GetIndex().ToString();
        line += ",none";
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

        line += "," + (args.interactor.name.ToLower().Contains("left") ? "Left" : "Right");
        line += "," + t5m.colour;
        line += "," + args.interactable.GetComponent<CubeColorController>().GetColour().ToString();
        line += "," + args.interactable.GetComponent<CubeColorController>().GetIndex().ToString();
        line += "," + (t5m.Compare(args.interactable.GetComponent<CubeColorController>().GetColour()) ? 
            "Success" : "Fail");
        Write(this.filename, line, this.filename);
    }

    public void LogChangeEvent(CubeColorController.CubeChangeArgs args)
    {
        // Must be ON the cube
        State state = State.ColourChange;
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        XRBaseInteractor interactor = interactable.selectingInteractor;
        string instanceID = interactable.GetInstanceID().ToString();
        ObjectProperties props = interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        line += "," + (interactor != null ? interactor.name : "none");
        line += "," + (interactor != null ? interactor.GetInstanceID().ToString() : "none");

        line += "," + "none";
        line += "," + t5m.colour;
        line += "," + args.newColour;
        line += "," + args.newIndex;
        line += ",none";
        Write(this.filename, line, this.filename);
    }

}
