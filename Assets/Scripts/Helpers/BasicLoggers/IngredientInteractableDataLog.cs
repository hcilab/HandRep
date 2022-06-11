using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IngredientInteractableDataLog : BaseDataLog
{

    // Applies to:
    // Mushroom, Feather, Dusts, PuzzleCube, Bottle, Cork.
    // Anything you pick up
    private enum State
    {
        None,
        Selected,
        Unselected,
        TimeoutDestroy
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
        if(w != null && !w.Equals(""))
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
        if (!newS.Equals("Spawner"))
        {
            if(cleanName != null && type != null)
                if(!(cleanName.Equals("") && type.Equals("") && newS.Equals("MortarSocket")))
                    Write(this.filename, line, this.filename);
        }
    }

    public void LogTimeoutEvent()
    {
        string prevS = "";
        string newS = "";
        State state = State.TimeoutDestroy;
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        string instanceID = interactable.GetInstanceID().ToString();
        ObjectProperties props = interactable.GetComponent<ObjectProperties>();
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
        Write(this.filename, line, this.filename);
    }
}
