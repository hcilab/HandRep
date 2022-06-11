using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketLogger : BaseLogger
{//PATTERN
    // handtype, sourcetype, event_type, eventargstype, time,
    // event interactor name, <- tag, <- id,
    //event interactable name, event interactable tag, event interactable id,
    // interactable selected bool,
    // interactor selecting interactable name, <- tag, <- id,
    // interactable hovered bool,
    // interactor hovering interactable name, <- tag, <- id,

    // ObjectProperties separated by |
    // socketProperties sep by |

    public void LogInteractEvent(BaseInteractionEventArgs args)
    {
        string type = "D_socket";
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
            hoveringInteractable = targets.Count > 0 ? targets[0] : null;
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
        string sockprops = "null";

        if(TryGetComponent<XRPropertySocket>(out XRPropertySocket psock))
        {
            sockprops = PropsToString(psock.targetProperties);
        }
        sockprops = sockprops.Replace(",", "|");
        line += "," + sockprops;

        WriteLog(line, timeMillis);
    }

    private string PropsToString(List<ObjectProperty> props)
    {
        string contents = "";
        foreach (ObjectProperty prop in props)
        {
            contents += prop.key + ": " + prop.value + ",";
        }
        contents.Remove(contents.Length - 1);
        return contents;
    }
}
