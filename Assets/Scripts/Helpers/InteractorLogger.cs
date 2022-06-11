using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractorLogger : BaseLogger
{    
    //PATTERN
    // handtype, sourcetype, event_type, eventargstype, time,
    // event interactor name, <- tag, <- id,
    //event interactable name, event interactable tag, event interactable id,
    // interactable selected bool,
    // interactor selecting interactable name, <- tag, <- id,
    // interactable hovered bool,
    // interactor hovering interactable name, <- tag, <- id,

    // ObjectProperties separated by |

    public void LogInteractEvent(BaseInteractionEventArgs args)
    {
        string type = "D_interactor";
        string eventType = "Interaction";
        float timeMillis = Time.time * 1000;
        XRBaseInteractor interactor = args.interactor;
        bool hasInteractable = interactor.selectTarget != null;
                
        XRBaseInteractable interactable = args.interactable;

        XRBaseInteractable selectingInteractable = interactor.selectTarget;
        bool isHovered = interactor.isHoverActive;
        XRBaseInteractable hoveringInteractable = null;
        if (isHovered)
        {
            List<XRBaseInteractable> targets = new List<XRBaseInteractable>();
            interactor.GetHoverTargets(targets);
            hoveringInteractable = targets.Count > 0 ?  targets[0] : null;
        }

        string properties = "null";

        string interactableInfo = "null,null,null";
        if (interactable)
        {
            interactableInfo = interactor.name + "," + interactor.tag + "," + interactor.GetInstanceID();
            if (interactable.TryGetComponent<ObjectProperties>(out ObjectProperties props))
            {
                properties = props.ToString();
            }
        }

        string selectInteractableInfo = "null,null,null";
        if (hasInteractable)
        {
            selectInteractableInfo = selectingInteractable.name + "," + selectingInteractable.tag + "," + selectingInteractable.GetInstanceID();
        }

        string hoverInteractableInfo = "null,null,null";
        if (isHovered && hoveringInteractable != null)
        {
            hoverInteractableInfo = hoveringInteractable.name + "," + hoveringInteractable.tag + "," + hoveringInteractable.GetInstanceID();
        }
        string line = settings.GetCurrentHandModelSet().ToString() + ",";
        line += type + "," + eventType + "," + args.GetType() + "," + timeMillis;
        line += "," + interactor.name + "," + interactor.tag + "," + interactor.GetInstanceID();
        line += "," + interactableInfo;
        line += "," + hasInteractable + "," + selectInteractableInfo;
        line += "," + isHovered + "," + hoverInteractableInfo;
        properties = properties.Replace(",", "|");
        line += "," + properties;
        line += ",null";// targetprops area

        WriteLog(line, timeMillis);
    }
}
