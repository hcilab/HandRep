using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TemperatureDataLog : BaseDataLog
{
    private enum State
    {
        None,
        Selected,
        Unselected,
        Rotating
    }

    /*private enum Status
    {
        NoRange,
        InRange,
        OutOfRange
    }*/

    [SerializeField] private DialReceiver dial;
    [SerializeField] private AlchemyRecipeTask task;
    
    public void Start()
    {
        string headers = this.GetMandatoryHeaders();
        headers += ",State";
        headers += ",Previous Selector";
        headers += ",Previous Selector ID";
        headers += ",Current Selector";
        headers += ",Current Selector ID";
        headers += ",Current Value";
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

        /*Status s = Status.NoRange;
        string rangeMin = "none";
        string rangeMax = "none";
        
        if(task)
        {
            if(task.GetTaskActive())
            {
                task.GetTaskIngredients().TryGetValue("temperature", out Ingredient temp);
                if(temp != null)
                {
                    rangeMin = temp.minQuantity.ToString("F2");
                    rangeMax = temp.maxQuantity.ToString("F2");
                    s = IsInBetween(dial.GetCurrentValue(), temp.minQuantity, temp.maxQuantity) ?
                        Status.InRange : Status.OutOfRange;
                }
            }
        }*/

        string line = "";
        line += state.ToString();
        line += "," + prevS;
        line += "," + prevSID;
        line += "," + newS;
        line += "," + newSID;
        //line += "," + s.ToString();
        line += "," + dial.GetCurrentValueString();
        //line += "," + rangeMin;
        //line += "," + rangeMax;
        Write(this.filename, line, this.filename);
    }

    public void LogRotateEvent(float rawVal, float normalizedVal)
    {
        string prevS = "";
        string newS = "";
        State state = State.Rotating;

        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        prevS = "none";
        string prevSID = "none";
        newS = interactable.isSelected ? interactable.selectingInteractor.tag : "none";
        string newSID = interactable.isSelected ? interactable.selectingInteractor.GetInstanceID().ToString() : "none";
        //Status s = Status.NoRange;
        //string rangeMin = "none";
        //string rangeMax = "none";

        /*if (task)
        {
            if (task.GetTaskActive())
            {
                task.GetTaskIngredients().TryGetValue("temperature", out Ingredient temp);
                if (temp != null)
                {
                    rangeMin = temp.minQuantity.ToString("F2");
                    rangeMax = temp.maxQuantity.ToString("F2");
                    s = IsInBetween(normalizedVal, temp.minQuantity, temp.maxQuantity) ?
                        Status.InRange : Status.OutOfRange;
                }
            }
        }*/

        string line = "";
        line += state.ToString();
        line += "," + prevS;
        line += "," + prevSID;
        line += "," + newS;
        line += "," + newSID;
        //line += "," + s.ToString();
        line += "," + normalizedVal;
        //line += "," + rangeMin;
        //line += "," + rangeMax;
        Write(this.filename, line, this.filename);
    }

    private bool IsInBetween(float a, float min, float max )
    {
        return a <= max && a >= min;
    }

}
