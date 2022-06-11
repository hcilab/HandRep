using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScaleDataLog : BaseDataLog
{
    private enum State
    {
        None,
        Placed,
        Removed
    }

    public void Start()
    {
        string headers = this.GetMandatoryHeaders();
        headers += ",State";
        headers += ",Object ID";
        headers += ",Object Name";
        headers += ",Object Ingredient Type";
        headers += ",Object Weight";

        WriteHeader(this.filename, headers, this.filename);
    }
    public override void LogEvent(BaseInteractionEventArgs args)
    {
        State state = State.None;
        if (args is SelectEnterEventArgs)
            state = State.Placed;
        else if (args is SelectExitEventArgs)
            state = State.Removed;
        else
            return; ///////////
        string instanceID = args.interactable.GetInstanceID().ToString();
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");
        string type = props.GetProperty("ingredient_type");
        string weight = float.Parse(props.GetProperty("weight")).ToString("F2");

        string line = "";
        line += state.ToString();
        line += "," + instanceID;
        line += "," + cleanName;
        line += "," + type;
        line += "," + weight;

        Write(this.filename, line, this.filename);
    }
}
