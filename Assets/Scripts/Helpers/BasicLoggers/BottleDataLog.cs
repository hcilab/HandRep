using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BottleDataLog : BaseDataLog
{
    private enum State
    {
        None,
        Selected,
        Unselected,
        Corked,
        UnCorked,
        PouringStart,
        PouringEnd,
        BottleFill,
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
        headers += ",CustomContents";
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

        string contents = "none";
        if(TryGetComponent<CustomPotionManager>(out CustomPotionManager cpm)) {
            if(cpm.GetBottleRecipe().Count > 0)
                contents = "";
            foreach(Ingredient i in cpm.GetBottleRecipe().Values)
            {
                contents += i.ToString() + "|";
            }
        }
        line += "," + contents;
        Write(this.filename, line, this.filename);
    }

    public void LogCorkedEvent(BaseInteractionEventArgs args)
    {
        // make the assumption that the OTHER hand that isnt current selector is the one corking it
        string prevS = "";
        string newS = "";
        State state = State.None;
        if (args is SelectEnterEventArgs)
            state = State.Corked;
        else if (args is SelectExitEventArgs)
            state = State.UnCorked;
        else
            return; ///////////
        XRBaseInteractable interactableBOTTLE = GetComponent<XRBaseInteractable>();
        string instanceID = interactableBOTTLE.GetInstanceID().ToString();
        ObjectProperties props = interactableBOTTLE.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");
        string type = props.GetProperty("ingredient_type");
        string w = props.GetProperty("weight");
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");

        prevS = "none";
        string prevSID = "none";
        newS = interactableBOTTLE.isSelected ? interactableBOTTLE.selectingInteractor.tag : "none";
        string newSID = interactableBOTTLE.isSelected ? interactableBOTTLE.selectingInteractor.GetInstanceID().ToString() : "none";

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
        string contents = "none";
        if (TryGetComponent<CustomPotionManager>(out CustomPotionManager cpm))
        {
            if (cpm.GetBottleRecipe().Count > 0)
                contents = "";
            foreach (Ingredient i in cpm.GetBottleRecipe().Values)
            {
                contents += i.ToString() + "|";
            }
        }
        line += "," + contents;
        Write(this.filename, line, this.filename);
    }

    public void LogPourEvent(bool started)
    {
        string prevS = "";
        string newS = "";
        State state = started ? State.PouringStart : State.PouringEnd;
        XRBaseInteractable interactableBOTTLE = GetComponent<XRBaseInteractable>();
        string instanceID = interactableBOTTLE.GetInstanceID().ToString();
        ObjectProperties props = interactableBOTTLE.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");
        string type = props.GetProperty("ingredient_type");
        string w = props.GetProperty("weight");
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");

        prevS = "none";
        string prevSID = "none";
        newS = interactableBOTTLE.isSelected ? interactableBOTTLE.selectingInteractor.tag : "none";
        string newSID = interactableBOTTLE.isSelected ? interactableBOTTLE.selectingInteractor.GetInstanceID().ToString() : "none";

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
        string contents = "none";
        if (TryGetComponent<CustomPotionManager>(out CustomPotionManager cpm))
        {
            if (cpm.GetBottleRecipe().Count > 0)
                contents = "";
            foreach (Ingredient i in cpm.GetBottleRecipe().Values)
            {
                contents += i.ToString() + "|";
            }
        }
        line += "," + contents;
        Write(this.filename, line, this.filename);
    }

    public void LogFillEvent(Dictionary<string, Ingredient> ingredients)
    {
        string prevS = "";
        string newS = "";
        State state = State.BottleFill;
        XRBaseInteractable interactableBOTTLE = GetComponent<XRBaseInteractable>();
        string instanceID = interactableBOTTLE.GetInstanceID().ToString();
        ObjectProperties props = interactableBOTTLE.GetComponent<ObjectProperties>();
        string cleanName = props.GetProperty("clean_name");
        string type = props.GetProperty("ingredient_type");
        string w = props.GetProperty("weight");
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");

        prevS = "none";
        string prevSID = "none";
        newS = interactableBOTTLE.isSelected ? interactableBOTTLE.selectingInteractor.tag : "none";
        string newSID = interactableBOTTLE.isSelected ? interactableBOTTLE.selectingInteractor.GetInstanceID().ToString() : "none";

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
        string contents = "none";
        if (ingredients.Count > 0)
        {
            contents = "";
            foreach (Ingredient i in ingredients.Values)
            {
                contents += i.ToString() + "|";
            }
        }
        
        line += "," + contents;
        Write(this.filename, line, this.filename);
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
        string contents = "none";
        if (TryGetComponent<CustomPotionManager>(out CustomPotionManager cpm))
        {
            if (cpm.GetBottleRecipe().Count > 0)
                contents = "";
            foreach (Ingredient i in cpm.GetBottleRecipe().Values)
            {
                contents += i.ToString() + "|";
            }
        }
        line += "," + contents;
        Write(this.filename, line, this.filename);
    }
}
