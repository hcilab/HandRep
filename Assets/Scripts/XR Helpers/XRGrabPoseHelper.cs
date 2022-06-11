using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
[RequireComponent(typeof(XRPoseHelper))]
public class XRGrabPoseHelper : MonoBehaviour
{


    private XRPoseHelper PoseHelper => GetComponent<XRPoseHelper>();
    private XRBaseInteractor interactor;
    private XRBaseInteractable interactable => GetComponent<XRBaseInteractable>();

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(GrabEnter);
        interactable.selectExited.AddListener(GrabExit);
    }
    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(GrabEnter);
        interactable.selectExited.RemoveListener(GrabExit);
    }

    private void GrabEnter(SelectEnterEventArgs args)
    {
        interactor = GetComponent<XRBaseInteractable>().selectingInteractor;
        PoseHelper.ShowPose(interactor);
    }
    private void GrabExit(SelectExitEventArgs args)
    {
        PoseHelper.HidePose();
    }
}
