using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class XRVelocityGrabInteractable : XRGrabInteractable
{
    private Transform previousParent;
    public bool enableCustomBehaviour = true;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        SetParentToXRRig();
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        SetParentToPrevious();
        base.OnSelectExited(args);
    }

    public void SetParentToXRRig()
    {
        if (!enableCustomBehaviour)
            return;
        if(transform && transform.parent)
            previousParent = transform.parent;
        if(selectingInteractor && selectingInteractor.transform)
            transform.SetParent(selectingInteractor.transform);
    }

    public void SetParentToPrevious()
    {
        if (!enableCustomBehaviour)
            return;
        if (!this.isSelected)
            try
            {
                if(!this.retainTransformParent && this.gameObject.activeInHierarchy)
                {
                    transform.SetParent(null);
                    return;
                }
                if (previousParent != null && transform.gameObject.activeSelf && previousParent.gameObject.activeSelf && 
                    transform.gameObject.activeInHierarchy && previousParent.gameObject.activeInHierarchy
                    && !previousParent.CompareTag("Spawner"))
                    transform.SetParent(previousParent);
                else
                    transform.SetParent(null);
            }
            catch {
                return;
            }
    }
}
