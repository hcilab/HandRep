using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum DialAxis
{
    X,
    Y,
    Z
}

// implement min and max

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(XRPoseHelper))]
public class XRGrabRotatable : MonoBehaviour
{

    [Range(10, 45)]
    [SerializeField] private int rotationAmount = 10;
    [Range(7.0f, 30.0f)]
    [SerializeField] private float handTolerance = 7f;

    private float initAngle = 0f;
    private bool requireInitAngle = true;
    private bool getHandRotation = false;

    [SerializeField] bool invert = false;

    [SerializeField] Transform dialTransform;

    [SerializeField] private DialAxis rotateAxis = DialAxis.X;

    private XRPoseHelper PoseHelper => GetComponent<XRPoseHelper>();


    private XRBaseInteractor interactor;
    private XRGrabInteractable grabInteractor => GetComponent<XRGrabInteractable>();

    [Tooltip("Minimum will be 0 degrees")]
    [SerializeField] private bool EnableMax = false;
    //[SerializeField] private bool EnableMax = false;
    [Range(0.0f, 360.0f)]
    [SerializeField] private float maxRotation = 360f;
    //[Range(0.0f, 340.0f)]
    //[SerializeField] private float minRotation = 0f;

    private float lastVal = 0f;

    private void OnEnable()
    {
        grabInteractor.selectEntered.AddListener(GrabEnter);
        grabInteractor.selectExited.AddListener(GrabExit);
        grabInteractor.hoverEntered.AddListener(HoverEnter);
        grabInteractor.hoverExited.AddListener(HoverExit);
    }
    private void OnDisable()
    {
        grabInteractor.selectEntered.RemoveListener(GrabEnter);
        grabInteractor.selectExited.RemoveListener(GrabExit);
        grabInteractor.hoverEntered.RemoveListener(HoverEnter);
        grabInteractor.hoverExited.RemoveListener(HoverExit);
    }

    private void HoverEnter(HoverEnterEventArgs args)
    {
        args.interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;
    }

    private void HoverExit(HoverExitEventArgs args)
    {
        if (!GetComponent<XRGrabInteractable>().isSelected)
            args.interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;//FindObjectOfType<Settings>().HideControllersOnSelect;
    }

    private void GrabEnter(SelectEnterEventArgs args)
    {
        interactor = GetComponent<XRGrabInteractable>().selectingInteractor;
        interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;
        getHandRotation = true;
        initAngle = 0f;
        SetHandModelVisibility(true);
    }
    private void GrabExit(SelectExitEventArgs args)
    {
        getHandRotation = false;
        requireInitAngle = true;
        SetHandModelVisibility(false);
        if (!GetComponent<XRGrabInteractable>().isHovered)
            interactor.GetComponent<XRDirectInteractor>().hideControllerOnSelect = true;//FindObjectOfType<Settings>().HideControllersOnSelect;

    }
    private void SetHandModelVisibility(bool visibility)
    {
        if (visibility)
        {
            PoseHelper.ShowPose(interactor);
        }
        else if (!visibility)
        {
            PoseHelper.HidePose();
        }
    }

    void Update()
    {
        if (getHandRotation)
        {
            float angle = GetInteractorRotation();
            GetRotationDistance(angle);
        }
    }

    public float GetInteractorRotation() =>  interactor.GetComponent<Transform>().localEulerAngles.z;
    

    private void GetRotationDistance(float angle)
    {

        if(!requireInitAngle)
        {
            float diff = Mathf.Abs(initAngle - angle);

            if (diff > handTolerance)
            {
                if(diff > 270f)
                {
                    float check;
                    if(initAngle < angle)
                    {
                        check = CheckAngle(angle, initAngle);
                        if(check < handTolerance)
                        {
                            return;
                        }
                        else
                        {
                            RotateDial(true);
                            initAngle = angle;
                        }
                    }
                    else if (initAngle > angle)
                    {
                        check = CheckAngle(angle, initAngle);
                        if(check < handTolerance)
                        {
                            return;
                        }
                        else
                        {
                            RotateDial(false);
                            initAngle = angle;
                        }
                    }
                }
                else
                {
                    if (initAngle < angle)
                    {
                        RotateDial(true);
                        initAngle = angle;
                    }
                    else if (initAngle > angle)
                    {
                        RotateDial(false);
                        initAngle = angle;
                    }
                }
            }
        }
        else
        {
            requireInitAngle = false;
            initAngle = angle;
        }
    }

    private float CheckAngle(float current, float init) => (360f - current) + init;

    private float GetNormalizedInvertedAngle(float angle)
    {
        if (invert)
        {
            angle = 360f - angle;
            if (angle > 359f)
            {
                angle = 0f;
            }
        }
        return angle;
    }
    private bool CheckBounds(float raw_angle, float orig_angle, float amount)
    {
        if(!EnableMax)
        {
            return true;
        }
        float angle = raw_angle;
        float prev_angle = orig_angle;
        if (invert)
        {
            angle = 360f - angle;
            prev_angle = 360f - prev_angle;
            if (angle > 359f)
            {
                angle = 0f;
            }
            if (prev_angle > 359f)
            {
                prev_angle = 0f;
            }
        }



        /**
        if(EnableMax && !EnableMin && angle > maxRotation + rotationAmount) 
        {
            return true;
        }

        if(!EnableMax && EnableMin && angle < minRotation - rotationAmount)
        {
            return true;
        }
        */

        /*

        if (EnableMax && angle > maxRotation || EnableMin && angle < minRotation)
        {
            return false;
        }
        */

        /*
        if(EnableMax && !EnableMin && (angle > maxRotation || angle >= maxRotation - rotationAmount) ) 
        {
            return false;
        }

        if (!EnableMax && EnableMin && (angle < minRotation || angle >= 360f - rotationAmount)) 
        {
            return false;
        }
        */

        if ((EnableMax && angle > maxRotation) )
        {
            return false;
        }

        if(EnableMax && angle < 0f + rotationAmount && prev_angle >= 360f - rotationAmount)
        { // cw
            return false;
        }
        
        if (EnableMax && angle >= 360f - rotationAmount && prev_angle < 0f + rotationAmount && maxRotation >= 360f - rotationAmount)
        { // ccw
            return false;
        }
        return true;
        
    }

    private void RotateDial(bool clockwise)
    {
        float amount = clockwise ? rotationAmount : -rotationAmount;
        float angle = 0f;
        Vector3 newEulerAngles;
        switch (rotateAxis)
        {
            case DialAxis.X:
                newEulerAngles = new Vector3(Mathf.Round(dialTransform.localEulerAngles.x + amount), dialTransform.localEulerAngles.y, dialTransform.localEulerAngles.z);
                angle = newEulerAngles.x;
                if (CheckBounds(angle, dialTransform.localEulerAngles.x, amount))
                    dialTransform.localEulerAngles = newEulerAngles;
                else
                    return;
                break;
            case DialAxis.Y:
                newEulerAngles = new Vector3(dialTransform.localEulerAngles.x, Mathf.Round(dialTransform.localEulerAngles.y + amount), dialTransform.localEulerAngles.z);
                angle = newEulerAngles.y;
                if (CheckBounds(angle, dialTransform.localEulerAngles.y, amount))
                    dialTransform.localEulerAngles = newEulerAngles;
                else
                    return;
                break;
            case DialAxis.Z:
                newEulerAngles = new Vector3(dialTransform.localEulerAngles.x, dialTransform.localEulerAngles.y, Mathf.Round(dialTransform.localEulerAngles.z + amount));
                angle = newEulerAngles.z;
                if (CheckBounds(angle, dialTransform.localEulerAngles.z, amount))
                    dialTransform.localEulerAngles = newEulerAngles;
                else
                    return;
                break;
        }
        if (grabInteractor.isSelected)
        {
            if (TryGetComponent<IDial>(out IDial idial))
            {
                if (invert)
                {
                    angle = 360f - angle;
                    if (angle > 359f)
                    {
                        angle = 0f;
                    }
                }
                if (angle != lastVal)
                {
                    foreach (IDial dial in GetComponents<IDial>())
                    {
                        dial.DialUpdate(angle);
                    }
                }
                lastVal = angle;
            }
        }
    }
}
