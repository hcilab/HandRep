using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T3DataLog : BaseDataLog
{
    [SerializeField] T3_ValveManager vm;
    [SerializeField] DialReceiver dr;
    [SerializeField] XRBaseInteractable interactable;
    private enum State
    {
        None,
        Grabbed,
        LetGo,
        Rotate,
        Submit
    }

    public void Start()
    {
        string headers = this.GetMandatoryHeaders();
        headers += ",State";
        headers += ",Source Selector";
        headers += ",Source Selector ID";
        headers += ",Requested Min";
        headers += ",Requested Max";
        headers += ",Value";
        headers += ",Match";
        WriteHeader(this.filename, headers, this.filename);
    }

    public void LogGrabEvent(SelectEnterEventArgs args)
    {
        State state = State.Grabbed;
        
        string line = "";
        line += state.ToString();
        string tag = (args.interactor != null ? args.interactor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + tag;
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        line += "," + vm.min.ToString();
        line += "," + vm.max.ToString();
        line += "," + dr.GetCurrentValueString();
        line += "," + "none";

        Write(this.filename, line, this.filename);
    }

    public void LogLetGoEvent(SelectExitEventArgs args)
    {
        State state = State.LetGo;
        string line = "";
        line += state.ToString();
        string tag = (args.interactor != null ? args.interactor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + tag;
        line += "," + (args.interactor != null ? args.interactor.GetInstanceID().ToString() : "none");
        line += "," + vm.min.ToString();
        line += "," + vm.max.ToString();
        line += "," + dr.GetCurrentValueString();
        line += "," + "none";
        Write(this.filename, line, this.filename);
    }

    public void LogRotateEvent(float raw, float normalized)
    {
        string val = (normalized).ToString("f2");
        State state = State.Rotate;
        string line = "";
        line += state.ToString();
        string tag = (interactable.selectingInteractor != null ? interactable.selectingInteractor.tag : "none");
        if (tag.Equals("Spawner"))
        {
            return;
        }
        line += "," + tag;
        line += "," + (interactable.selectingInteractor != null ? interactable.selectingInteractor.GetInstanceID().ToString() : "none");
        line += "," + vm.min.ToString();
        line += "," + vm.max.ToString();
        line += "," + val;
        line += "," + "none";
        //line += "," + (vm.Compare(val) ? "Success" : "Fail");
        Write(this.filename, line, this.filename);
    }
    public void LogSubmitEvent()
    {
        string val = dr.GetCurrentValueString();
        State state = State.Submit;
        string line = "";
        line += state.ToString();
        string tag = "none";

        line += "," + tag;
        line += "," + "none";
        line += "," + vm.min.ToString();
        line += "," + vm.max.ToString();
        line += "," + val;
        line += "," + (vm.Compare(val) ? "Success" : "Fail");
        Write(this.filename, line, this.filename);
    }
}
