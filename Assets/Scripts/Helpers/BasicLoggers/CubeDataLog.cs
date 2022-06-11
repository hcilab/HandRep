using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeDataLog : BaseDataLog
{
    private enum State
    {
        None,
        Selected,
        Unselected,
        ColourChange
    }
    public void Start()
    {
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
        headers += ",Previous Colour";
        headers += ",Previous Side";
        headers += ",Current Colour";
        headers += ",Current Side";
        WriteHeader(this.filename, headers, this.filename);
    }
    public override void LogEvent(BaseInteractionEventArgs args)
    {
        string prevS = "";
        string newS = "";
        State state = State.None;
        if (args is SelectEnterEventArgs)
            state = State.Selected;
        else if (args is SelectExitEventArgs)
            state = State.Unselected;
        else
            return; ///////////
        string instanceID = args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");
        string type = props.GetProperty("ingredient_type");
        string w = props.GetProperty("weight");
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

        CubeColorController ccc = args.interactable.GetComponent<CubeColorController>();
        line += ",none";
        line += ",none";
        line += "," + ccc.GetColour().ToString();
        line += "," + ccc.GetIndex().ToString();

        Write(this.filename, line, this.filename);
    }

    public void LogChange(CubeColorController.CubeChangeArgs args)
    {
        string prevS = "";
        string newS = "";
        State state = State.ColourChange;
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        string instanceID = interactable.GetInstanceID().ToString();
        ObjectProperties props = interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");
        string type = props.GetProperty("ingredient_type");
        string w = props.GetProperty("weight");
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");

        // prev is same as current
        XRBaseInteractor interactor = interactable.selectingInteractor;
        prevS = interactor.tag;
        string prevSID = interactor.GetInstanceID().ToString();
        newS = interactable.selectingInteractor.tag;
        string newSID = interactable.selectingInteractor.GetInstanceID().ToString();

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

        line += "," + args.oldColour.ToString();
        line += "," + args.oldIndex.ToString();
        line += "," + args.newColour.ToString();
        line += "," + args.newIndex.ToString();

        Write(this.filename, line, this.filename);
    }
}
