using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EnchantDataLog : BaseDataLog
{
    [SerializeField] private XRBaseInteractable handle;

    private enum State
    {
        None,
        Selected,
        Unselected,
        EnchantItem,
        MovingForward,
        MovingBackward
    }
    // should i add a trigger area for detecting if an item is added to or removed
    // from the dish?

    public void Start()
    {
        string headers = this.GetMandatoryHeaders();
        headers += ",State";
        headers += ",Object ID"; // not none if enchant
        headers += ",Object Name"; // not none if enchant
        headers += ",Object Ingredient Type"; // not none if enchant
        headers += ",Object Weight"; // not none if enchant
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
        string instanceID = "none";
        string cleanName = "none";
        string type = "none";
        string weight = "none";

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
    public void LogMoveEvent(bool movingFW)
    {
        string prevS = "";
        string newS = "";
        State state = movingFW ? State.MovingForward : State.MovingBackward;
        string instanceID = "none";
        string cleanName = "none";
        string type = "none";
        string weight = "none";



        prevS = "none";
        string prevSID = "none";
        newS = handle.isSelected ? handle.selectingInteractor.tag : "none";
        string newSID = handle.isSelected ? handle.selectingInteractor.GetInstanceID().ToString() : "none";

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

    public void LogEnchantEvent(BaseInteractionEventArgs args)
    {
        string prevS = "";
        string newS = "";
        State state = State.EnchantItem;
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
        Write(this.filename, line, this.filename);
    }


}
