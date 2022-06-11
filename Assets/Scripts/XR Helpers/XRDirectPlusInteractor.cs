using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDirectPlusInteractor : XRDirectInteractor
{
    [SerializeField] bool allowDirectInteractorTake = false;
    [SerializeField] bool enableMaxDistance = true;
    [SerializeField] float maxDistanceSelect = 1f;

    public override bool CanSelect(XRBaseInteractable interactable)
    {

        if (!allowDirectInteractorTake && interactable.isSelected && interactable.selectingInteractor is XRDirectInteractor
            && interactable.selectingInteractor != this)
        {
            return base.CanSelect(interactable) && false;
        }
        else
            return base.CanSelect(interactable);
    }

    private void FixedUpdate()
    {
        if (enableMaxDistance && this.selectTarget != null && Vector3.Distance(this.transform.position, this.selectTarget.transform.position) > maxDistanceSelect)
        {
            this.interactionManager.CancelInteractorSelection(this);
        }
    }

    public void SetMaxDistance(float distance)
    {
        this.maxDistanceSelect = distance;
    }

}
