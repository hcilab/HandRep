using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CubeLogger : BaseLogger
{
    //PATTERN
    // handtype, sourcetype, event_type, eventargstype, time, event interactable name, <- tag, <- id,
    //event interactor name, event interactor tag, event interactor id,
    // interactable selected bool,
    // interactable selecting interactor name, <- tag, <- id,
    // interactable hovered bool,
    // interactable hovering interactor name, <- tag, <- id,
    // ObjectProperties separated by |
    // null,
    // Colour
    // Side Index

    public void LogCubeEvent(XRBaseInteractable interactable, CubeColorController.GemColour color, int sideIndex, string mod = "")
    {
        string type = "D_cube";
        string eventType = "CubeUpdate";
        if (!mod.Equals(""))
        {
            eventType += ":" + mod;
        }
        float timeMillis = Time.time * 1000;
        string interactorInfo = "null,null,null";
        bool isSelected = interactable.isSelected;
        XRBaseInteractor selectingInteractor = interactable.selectingInteractor;
        bool isHovered = interactable.isHovered;
        XRBaseInteractor hoveringInteractor = null;
        if (isHovered)
            hoveringInteractor = interactable.hoveringInteractors[0];
        string selectInteractorInfo = "null,null,null";
        if (isSelected)
        {
            selectInteractorInfo = selectingInteractor.name + "," + selectingInteractor.tag + "," + selectingInteractor.GetInstanceID();
        }
        string properties = "null";
        if (interactable.TryGetComponent<ObjectProperties>(out ObjectProperties props))
        {
            properties = props.ToString();
        }
        string hoverInteractorInfo = "null,null,null";
        if (isHovered && hoveringInteractor != null)
        {
            hoverInteractorInfo = hoveringInteractor.name + "," + hoveringInteractor.tag + "," + hoveringInteractor.GetInstanceID();
        }
        string line = settings.GetCurrentHandModelSet().ToString() + ",";
        line += type + "," + eventType + "," + "Activate" + "," + timeMillis;
        line += "," + interactable.name + "," + interactable.tag + "," + interactable.GetInstanceID();
        line += "," + interactorInfo;
        line += "," + isSelected + "," + selectInteractorInfo;
        line += "," + isHovered + "," + hoverInteractorInfo;
        properties = properties.Replace(",", "|");
        line += "," + properties;
        line += ",null";// targetprops area
        line += "," + color.ToString();
        line += "," + sideIndex;
        WriteLog(line, timeMillis);
    }
}
