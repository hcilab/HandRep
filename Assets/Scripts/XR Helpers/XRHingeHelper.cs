using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(HingeJoint))]
public class XRHingeHelper : MonoBehaviour
{
    private HingeJoint hinge => GetComponent<HingeJoint>();
    private XRGrabInteractable interactable => GetComponent<XRGrabInteractable>();

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(ForceRotation);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(ForceRotation);
    }

    private void ForceRotation(SelectEnterEventArgs args)
    {
        Debug.Log(args.interactor.transform.rotation);
        Debug.Log(args.interactor.transform.localRotation);
        Debug.Log(args.interactor.transform.eulerAngles);
        Debug.Log(args.interactor.transform.localEulerAngles);
        Debug.Log(hinge.angle);
        Debug.Log("TEST");
        // interactor Z 

        // when the controller holds the hinge, it immediately rotates to be the same angle as the hand local euler z (or rather the inverse since I flip the hinge)
        // need to stop this behaviour, give it an offset, or do something to the hand so it matches hinge? unable to change hinge angle.
    }

}
