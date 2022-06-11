using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableLogger : BaseLogger
{

    //PATTERN
    // handtype, sourcetype, event_type, eventargstype, time, event interactable name, <- tag, <- id,
    //event interactor name, event interactor tag, event interactor id,
    // interactable selected bool,
    // interactable selecting interactor name, <- tag, <- id,
    // interactable hovered bool,
    // interactable hovering interactor name, <- tag, <- id,

    // ObjectProperties separated by |

    public void LogInteractEvent(BaseInteractionEventArgs args)
    {
        string type = "D_interactable";
        string eventType = "Interaction";
        float timeMillis = Time.time * 1000;
        XRBaseInteractor interactor = args.interactor;
        XRBaseInteractable interactable = args.interactable;
        bool isSelected = interactable.isSelected;
        XRBaseInteractor selectingInteractor = interactable.selectingInteractor;
        bool isHovered = interactable.isHovered;
        XRBaseInteractor hoveringInteractor = null;
        if (isHovered) 
             hoveringInteractor = interactable.hoveringInteractors[0];

        // ignore spawners
        if(isSelected && selectingInteractor is XRQuantitySocket && 
            selectingInteractor.CompareTag("Spawner"))
        {
            return;
        }
        string properties = "null";
        if(interactable.TryGetComponent<ObjectProperties>(out ObjectProperties props))
        {
            properties = props.ToString();
        }

        string interactorInfo = "null,null,null";
        if(interactor)
        {
            interactorInfo = interactor.name + "," + interactor.tag + "," + interactor.GetInstanceID();
        }

        string selectInteractorInfo = "null,null,null";
        if (isSelected)
        {
            selectInteractorInfo = selectingInteractor.name + "," + selectingInteractor.tag + "," + selectingInteractor.GetInstanceID();
        }

        string hoverInteractorInfo = "null,null,null";
        if (isHovered && hoveringInteractor != null)
        {
            hoverInteractorInfo = hoveringInteractor.name + "," + hoveringInteractor.tag + "," + hoveringInteractor.GetInstanceID();
        }
        string line = settings.GetCurrentHandModelSet().ToString() + ",";
        line += type + "," + eventType + "," + args.GetType() + "," + timeMillis;
        line += "," + interactable.name + "," + interactable.tag + "," + interactable.GetInstanceID();
        line += "," + interactorInfo;
        line += "," + isSelected + "," + selectInteractorInfo;
        line += "," + isHovered + "," + hoverInteractorInfo;
        properties = properties.Replace(",", "|");
        line += "," + properties;
        line += ",null";// targetprops area
        WriteLog(line, timeMillis);
    } 

    public void LogDestroyEvent(string source = "")
    {
        if (!settings)
        {
            return;
        }
        string type = "D_interactable";
        string eventType = "Destroy";
        if (!source.Equals(""))
        {
            eventType += ":" + source;
        }
        float timeMillis = Time.time * 1000;
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
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
            if(!selectingInteractor)
            {
                return;
            }
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
        line += type + "," + eventType + "," + "null" + "," + timeMillis;
        line += "," + interactable.name + "," + interactable.tag + "," + interactable.GetInstanceID();
        line += "," + interactorInfo;
        line += "," + isSelected + "," + selectInteractorInfo;
        line += "," + isHovered + "," + hoverInteractorInfo;
        properties = properties.Replace(",", "|");
        line += "," + properties;
        line += ",null";// targetprops area
        WriteLog(line, timeMillis);
    }

    public void LogModifyEvent(string mod = "")
    {
        string type = "D_interactable";
        string eventType = "Modify";
        if (!mod.Equals(""))
        {
            eventType += ":" + mod;
        }
        float timeMillis = Time.time * 1000;
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
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
        line += type + "," + eventType + "," + "null" + "," + timeMillis;
        line += "," + interactable.name + "," + interactable.tag + "," + interactable.GetInstanceID();
        line += "," + interactorInfo;
        line += "," + isSelected + "," + selectInteractorInfo;
        line += "," + isHovered + "," + hoverInteractorInfo;
        properties = properties.Replace(",", "|");
        line += "," + properties;
        line += ",null";// targetprops area
        WriteLog(line, timeMillis);
    }
}
