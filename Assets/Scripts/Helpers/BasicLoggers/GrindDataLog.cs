using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class GrindDataLog : BaseDataLog
{
    [SerializeField] private XRSocketInteractor bowlSocket;
    [SerializeField] private XRBaseInteractable pestleHandle;

    private enum State
    {
        None,
        Selected,
        Unselected,
        ItemAdded, // selectors will be null for this
        RotationDone,
        GrindComplete
    }
    public void Start()
    {
        // 'Object' will be the currently grinding object
        string headers = this.GetMandatoryHeaders();
        headers += ",State";
        headers += ",Object ID";
        headers += ",Object Name";
        headers += ",Object Ingredient Type";
        headers += ",Object Weight";
        headers += ",Previous Selector";
        headers += ",Previous Selector ID";
        headers += ",Current Selector";
        headers += ",Current Selector ID";
        WriteHeader(this.filename, headers, this.filename);
    }
    public override void LogEvent(BaseInteractionEventArgs args)
    {
        // pestle handle only
        string prevS = "";
        string newS = "";
        State state = State.None;
        if (args is SelectEnterEventArgs)
            state = State.Selected;
        else if (args is SelectExitEventArgs)
            state = State.Unselected;
        else
            return; ///////////
        XRBaseInteractable obj = bowlSocket.selectTarget;
        string instanceID = obj != null ? obj.GetInstanceID().ToString() : "none";
        ObjectProperties props = obj != null ? obj.GetComponent<ObjectProperties>() : null;
        string cleanName = props != null ? props.GetProperty("clean_name") : "none";
        string type = props != null ? props.GetProperty("ingredient_type") : "none";
        string w = props != null ? props.GetProperty("weight") : null;
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");

        prevS = state.Equals(State.Unselected) ? args.interactor.tag : "none";
        string prevSID = state.Equals(State.Unselected) ? args.interactor.GetInstanceID().ToString() : "none";
        newS = args.interactable.isSelected ? args.interactable.selectingInteractor.tag : "none";
        string newSID = args.interactable.isSelected ? args.interactable.selectingInteractor.GetInstanceID().ToString() : "none";

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        line += "," + type;
        line += "," + weight;
        line += "," + prevS;
        line += "," + prevSID;
        line += "," + newS;
        line += "," + newSID;
        Write(this.filename, line, this.filename);
    }

    public void LogItemAddEvent(SelectEnterEventArgs args)
    {
        string prevS = "";
        string newS = "";
        State state = State.ItemAdded;

        string instanceID = args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");
        string type = props.GetProperty("ingredient_type");
        string w = props.GetProperty("weight");
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");

        prevS = "none";
        string prevSID = "none";
        newS = "none";
        string newSID = "none";

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        line += "," + type;
        line += "," + weight;
        line += "," + prevS;
        line += "," + prevSID;
        line += "," + newS;
        line += "," + newSID;
        if(!(cleanName != null  && type != null))
            Write(this.filename, line, this.filename);
    }

    private void LogGrindingEvents(State state)
    {
        string prevS = "";
        string newS = "";
        XRBaseInteractable obj = bowlSocket.selectTarget;
        string instanceID = obj != null ? obj.GetInstanceID().ToString() : "none";
        ObjectProperties props = obj != null ? obj.GetComponent<ObjectProperties>() : null;
        string cleanName = props != null ? props.GetProperty("clean_name") : "none";
        string type = props != null ? props.GetProperty("ingredient_type") : "none";
        string w = props != null ? props.GetProperty("weight") : null;
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");

        prevS = "none";
        string prevSID = "none";
        newS = pestleHandle.isSelected ? pestleHandle.selectingInteractor.tag : "none";
        string newSID = pestleHandle.isSelected ? pestleHandle.selectingInteractor.GetInstanceID().ToString() : "none";

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        line += "," + type;
        line += "," + weight;
        line += "," + prevS;
        line += "," + prevSID;
        line += "," + newS;
        line += "," + newSID;
        Write(this.filename, line, this.filename);
    }

    public void LogRotationDoneEvent()
    {
        LogGrindingEvents(State.RotationDone);
    }

    public void LogGrindCompleteEvent()
    {
        LogGrindingEvents(State.GrindComplete);
    }
}
