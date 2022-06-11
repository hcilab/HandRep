using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrinderLogger : BaseLogger
{   //PATTERN
    // handtype, sourcetype, event_type, eventargstype, time, event interactable name, <- tag, <- id,
    //event interactor name, event interactor tag, event interactor id,
    // interactable selected bool,
    // interactable selecting interactor name, <- tag, <- id,
    // interactable hovered bool,
    // interactable hovering interactor name, <- tag, <- id,

    // ObjectProperties separated by |

    public void LogGrindEvent(XRBaseInteractable interactable, string mod = "")
    {
        string type = "D_grind";
        string eventType = "Grinder";
        if (!mod.Equals(""))
        {
            eventType += ":" + mod;
        }
        float timeMillis = Time.time * 1000;
        string interactableInfo = "null,null,null";
        string interactorInfo = "null,null,null";
        string selectInteractorInfo = "null,null,null";
        bool isSelected = false;
        bool isHovered = false;
        string properties = "null";
        string hoverInteractorInfo = "null,null,null";
        if (interactable != null)
        {
            interactableInfo = interactable.name + "," + interactable.tag + "," + interactable.GetInstanceID();
            isSelected = interactable.isSelected;
            XRBaseInteractor selectingInteractor = interactable.selectingInteractor;
            isHovered = interactable.isHovered;
            XRBaseInteractor hoveringInteractor = null;
            if (isHovered)
                hoveringInteractor = interactable.hoveringInteractors[0];
            
            if (isSelected)
            {
                selectInteractorInfo = selectingInteractor.name + "," + selectingInteractor.tag + "," + selectingInteractor.GetInstanceID();
            }
            
            if (interactable.TryGetComponent<ObjectProperties>(out ObjectProperties props))
            {
                properties = props.ToString();
            }
            
            if (isHovered && hoveringInteractor != null)
            {
                hoverInteractorInfo = hoveringInteractor.name + "," + hoveringInteractor.tag + "," + hoveringInteractor.GetInstanceID();
            }
        }
        string line = settings.GetCurrentHandModelSet().ToString() + ",";
        line += type + "," + eventType + "," + "null" + "," + timeMillis;
        line += "," + interactableInfo;
        line += "," + interactorInfo;
        line += "," + isSelected + "," + selectInteractorInfo;
        line += "," + isHovered + "," + hoverInteractorInfo;
        properties = properties.Replace(",", "|");
        line += "," + properties;
        line += ",null";// targetprops area
        WriteLog(line, timeMillis);
    }

}
