using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(XRGrabInteractable))]
public class XRLeverHelper : MonoBehaviour
{
    XRGrabInteractable interactable => GetComponent<XRGrabInteractable>();
    Rigidbody rigidBody => GetComponent<Rigidbody>();

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(Grab);
        interactable.selectExited.AddListener(LetGo);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(Grab);
        interactable.selectExited.RemoveListener(LetGo);
    }

    private void LetGo(SelectExitEventArgs args)
    {
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void Grab(SelectEnterEventArgs args)
    {
        rigidBody.constraints = RigidbodyConstraints.None;
    }

    private void Awake()
    {
        LetGo(null);
    }
}
