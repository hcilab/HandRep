using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestDataLog : BaseDataLog
{
    private enum State
    {
        None,
        Selected,
        Unselected,
        Closed,
        Opened
    }
    public void Start()
    {
        string headers = this.GetMandatoryHeaders();
        headers += ",State";
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

        prevS = state.Equals(State.Unselected) ? args.interactor.tag : "none";
        string prevSID = state.Equals(State.Unselected) ? args.interactor.GetInstanceID().ToString() : "none";
        newS = args.interactable.isSelected ? args.interactable.selectingInteractor.tag : "none";
        string newSID = args.interactable.isSelected ? args.interactable.selectingInteractor.GetInstanceID().ToString() : "none";

        string line = "";
        line += state.ToString();
        line += "," + prevS;
        line += "," + prevSID;
        line += "," + newS;
        line += "," + newSID;
        Write(this.filename, line, this.filename);
    }

    public void LogChestEvent(bool isOpen)
    {
        string prevS = "";
        string newS = "";
        State state = isOpen ? State.Opened : State.Closed;

        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();

        prevS = "none";
        string prevSID = "none";
        newS = interactable.isSelected ? interactable.selectingInteractor.tag : "none";
        string newSID = interactable.isSelected ? interactable.selectingInteractor.GetInstanceID().ToString() : "none";

        string line = "";
        line += state.ToString();
        line += "," + prevS;
        line += "," + prevSID;
        line += "," + newS;
        line += "," + newSID;
        Write(this.filename, line, this.filename);
    }
}
