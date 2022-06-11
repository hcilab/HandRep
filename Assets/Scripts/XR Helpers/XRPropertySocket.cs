using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class XRPropertySocket : XRSocketInteractor
{

    [SerializeField] public bool disallowNonPropertiedObjects = false;

    [SerializeField]
    [Tooltip("All objects must match each key:value property in this list to be socketed")]
    public List<ObjectProperty> targetProperties = new List<ObjectProperty>();

    public override bool CanHover(XRBaseInteractable interactable)
    {
        return base.CanHover(interactable) && MatchProperty(interactable);
    }

    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return base.CanSelect(interactable) && MatchProperty(interactable);
    }

    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        if(this.CanHover(args.interactable) && 
                args.interactable.TryGetComponent<XRPoseHelper>(out XRPoseHelper poseHelper))
        {
            if(poseHelper.activeModel != null)
            {
                poseHelper.activeModel.transform.SetParent(args.interactable.selectingInteractor.transform);
            }
        }
        base.OnHoverEntering(args);
    }
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        if (this.CanHover(args.interactable) && 
            args.interactable.TryGetComponent<XRPoseHelper>(out XRPoseHelper poseHelper))
        {
            if (poseHelper.activeModel != null)
            {
                poseHelper.activeModel.transform.SetParent(
                    ((XRGrabInteractable)args.interactable).attachTransform);
            }
        }
        base.OnHoverExited(args);
    }

    private bool MatchProperty(XRBaseInteractable interactable)
    {
        ObjectProperties props;
        interactable.TryGetComponent<ObjectProperties>(out props);

        if(!props && disallowNonPropertiedObjects)
        {
            return false;
        }

        if (!props || this.targetProperties == null || this.targetProperties.Count == 0)
        {
            return true;
        }

        List<ObjectProperty> unmatched = new List<ObjectProperty>(this.targetProperties);

        foreach (ObjectProperty entry in props.properties)
        {
            foreach (ObjectProperty e2 in unmatched)
            {
                if (entry.Equals(e2))
                {
                    unmatched.Remove(e2);
                    break;
                }
            }
            if (unmatched.Count == 0)
            {
                return true;
            }
        }

        if (unmatched.Count > 0)
        {
            return false;
        }

        return true;
    }

    public void RemoveContents()
    {
        if(this.selectTarget)
        {
            Destroy(this.selectTarget.gameObject);
        }
    }
}
