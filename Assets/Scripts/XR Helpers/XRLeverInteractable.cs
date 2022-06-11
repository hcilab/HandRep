using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(HingeJoint))]
//[RequireComponent(typeof(LeverLogger))]
public class XRLeverInteractable : MonoBehaviour
{
    public UnityEvent OnFullPull = null;

    private HingeJoint hj => GetComponent<HingeJoint>();
    [SerializeField] float angleBuffer = 5f;
    [SerializeField] float minAngle = -35f;
    [SerializeField] float maxAngle = 35f;

    //private LeverLogger llog => GetComponent<LeverLogger>();
    [SerializeField] XRBaseInteractable interactable => GetComponent<XRBaseInteractable>();

    private bool inBuffer = false;
    public enum ActivateBehaviour
    {
        Min,
        Max,
        Both,
        None
    }

    [SerializeField] private ActivateBehaviour behaviour = ActivateBehaviour.None;
    private void Update()
    {
        bool inZone = false;
        switch (behaviour)
        {
            case ActivateBehaviour.None:
                return;
            case ActivateBehaviour.Both:
                inZone = hj.angle >= maxAngle - angleBuffer || hj.angle <= minAngle + angleBuffer;
                break;
            case ActivateBehaviour.Min:
                inZone = hj.angle <= minAngle + angleBuffer;
                break;
            case ActivateBehaviour.Max:
                inZone = hj.angle >= maxAngle - angleBuffer;
                break;
        }

        if(inBuffer && !inZone)
        {
            inBuffer = false;
            return;
        }

        if(!inBuffer && inZone)
        {
            inBuffer = true;
            if(OnFullPull != null)
            {
                /*if(llog)
                {
                    llog.LogLeverEvent(interactable.selectingInteractor);
                }*/
                OnFullPull.Invoke();
            }
        }
    }

    private void OnValidate()
    {
        
    }
}
