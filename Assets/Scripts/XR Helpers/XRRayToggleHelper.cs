using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRRayInteractor))]
public class XRRayToggleHelper : MonoBehaviour
{

    private XRRayInteractor rayInteractor => GetComponent<XRRayInteractor>();
    [SerializeField] private bool isEnabled = false;

    private void OnEnable()
    {
        Settings.OnRayToggleUpdate += ToggleRay;
    }

    private void OnDisable()
    {
        Settings.OnRayToggleUpdate -= ToggleRay;
    }

    private void ToggleRay(bool enabled)
    {
        isEnabled = enabled;
    }

    private void LateUpdate()
    {
        ApplyStatus();
    }

    private void ApplyStatus()
    {
        if(rayInteractor.enabled != isEnabled)
        {
            rayInteractor.enabled = isEnabled;
        }
    }
}
