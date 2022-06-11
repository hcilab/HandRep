using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

//[RequireComponent(typeof(ButtonLogger))]
public class XRButtonInteractable : XRBaseInteractable
{
    public UnityEvent OnPress = null;

    [SerializeField] private bool onlyHandCanPress = false;

    [Range(0.1f, 1.0f)]
    [SerializeField] float pressAmount = 0.5f;
    [SerializeField] float buttonHeight = 1f;
    private float yMin = 0.0f;
    private float yMax = 0.0f;
    private bool prevPress = false;

    [SerializeField] Collider collider;

    private float prevHandHeight = 0.0f;
    private XRBaseInteractor interactor = null;

    [SerializeField] AudioSource audioSourcePressPrevention;

    //private ButtonLogger blog => GetComponent<ButtonLogger>();

    protected override void OnEnable()
    {
        base.OnEnable();
        hoverEntered.AddListener(StartPress);
        hoverExited.AddListener(EndPress);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        hoverEntered.RemoveListener(StartPress);
        hoverExited.RemoveListener(EndPress);

    }

    public override bool IsSelectableBy(XRBaseInteractor interactor)
    { // if update replacing process is buggy, remove this, reinstate processinteractable
        return false;
    }

    private void StartPress(HoverEnterEventArgs args)
    {
        if(onlyHandCanPress && !args.interactor.TryGetComponent<XRDirectInteractor>(out XRDirectInteractor xdri))
        {
            return;
        }
        this.interactor = args.interactor;
        prevHandHeight = GetLocalYPos(this.interactor.transform.position);
    }

    private void EndPress(HoverExitEventArgs args)
    {
        this.interactor = null;
        prevHandHeight = 0.0f;
        prevPress = false;
        SetYPos(yMax);
    }

    private void Start()
    {
        SetMinMax();
    }

    private void SetMinMax()
    {
        // This won't cover wall buttons being pushed in only N amount
        // Since collider bounds are worldspace 
        yMin = transform.localPosition.y - (/*collider.bounds.size.y*/ buttonHeight * pressAmount);
        yMax = transform.localPosition.y;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    { // replace with update if buggy
        if(interactor)
        {
            float newHandHeight = GetLocalYPos(interactor.transform.position);
            float diff = prevHandHeight - newHandHeight;
            prevHandHeight = newHandHeight;
            float newPos = transform.localPosition.y - diff;
            SetYPos(newPos);

            CheckPress();
        }
    }

    private float GetLocalYPos(Vector3 pos)
    {
        Vector3 localPos = transform.root.InverseTransformPoint(pos);
        return localPos.y;
    }

    private void SetYPos(float pos)
    {
        Vector3 newPos = transform.localPosition;
        newPos.y = Mathf.Clamp(pos, yMin, yMax);
        transform.localPosition = newPos;
    }

    private void CheckPress()
    {
        bool isInPosition = InPosition();

        if (isInPosition && isInPosition != prevPress)
        {
            if (OnPress != null)
            {
                /*if(blog)
                {
                    blog.LogButtonEvent(this.interactor);
                }*/
                if(audioSourcePressPrevention && !audioSourcePressPrevention.isPlaying)
                    OnPress.Invoke();
                else if (!audioSourcePressPrevention)
                {
                    OnPress.Invoke();
                }
            }
        }
        prevPress = isInPosition;
    }

    private bool InPosition()
    {
        float inRange = Mathf.Clamp(transform.localPosition.y,
            yMin, yMin + 0.01f);
        return transform.localPosition.y == inRange;
    }
}
