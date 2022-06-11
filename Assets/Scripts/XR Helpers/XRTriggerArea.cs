using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using MyBox;

public class XRTriggerArea : XRBaseInteractor
{
    public enum TriggerAreaBehaviour
    {
        MustBeHeld,
        MustNotBeHeld,
        Any
    }

    private XRBaseInteractable current = null;
    [SerializeField] private TriggerAreaBehaviour behaviour = TriggerAreaBehaviour.MustNotBeHeld;
    [SerializeField] private bool compareProperties = false;

    [SerializeField]
    [Tooltip("All objects must match each key:value property in this list to be socketed")]
    public List<ObjectProperty> targetProperties = new List<ObjectProperty>();

    public bool OnlyOneTarget = true;
    private void OnTriggerEnter(Collider other)
    {
        SetInteractable(other);
    }

    private void OnTriggerExit(Collider other)
    {
        ClearInteractable(other);
    }

    private void SetInteractable(Collider other)
    {
        if (TryGetInteractable(other, out XRBaseInteractable interactable))
        {
            switch(behaviour)
            {
                case TriggerAreaBehaviour.MustBeHeld:
                    if (interactable.isSelected && 
                        interactable.selectingInteractor is XRBaseControllerInteractor)
                    {
                        if (current == null)
                        {
                            current = interactable;

                        }
                    }
                    return;
                case TriggerAreaBehaviour.MustNotBeHeld:
                    if(!interactable.isSelected)
                    {
                        if (current == null)
                        {
                            current = interactable;

                        }
                    }
                    return;
                case TriggerAreaBehaviour.Any:
                    if (current == null)
                    {
                        current = interactable;
                    }
                    return;
                default:
                    if (current == null)
                    {
                        current = interactable;
                    }
                    return;
            }
        }
    }

    private void ClearInteractable(Collider other)
    {
        if (TryGetInteractable(other, out XRBaseInteractable interactable))
        {
            if (current == interactable)
            {
                current = null;
            }
        }
    }

    private bool TryGetInteractable(Collider col, out XRBaseInteractable interactable)
    {
        interactable = interactionManager.GetInteractableForCollider(col);
        return interactable != null;
    }

    public override void GetValidTargets(List<XRBaseInteractable> targets)
    {
        if (OnlyOneTarget)
        {
            targets.Clear();
            targets.Add(current);
        }
    }

    
    public override bool CanHover(XRBaseInteractable interactable)
    {
        bool a = base.CanHover(interactable) && current == interactable;
        if(compareProperties) 
            a = a && MatchProperty(interactable);
        switch (behaviour)
        {
            case TriggerAreaBehaviour.MustBeHeld:
                a = a && interactable.isSelected && interactable.selectingInteractor is XRBaseControllerInteractor;
                break;
            case TriggerAreaBehaviour.MustNotBeHeld:
                a = a && !interactable.isSelected;
                break;
            case TriggerAreaBehaviour.Any:
                break;
            default:
                break;
        }
        return a;
    }
    
    public override bool CanSelect(XRBaseInteractable interactable)
    {
        return false;
    }

    private bool MatchProperty(XRBaseInteractable interactable)
    {
        if (!compareProperties)
        {
            return true;
        }

        ObjectProperties props;
        interactable.TryGetComponent<ObjectProperties>(out props);

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
}
