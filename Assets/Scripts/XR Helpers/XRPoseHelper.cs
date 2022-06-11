using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class XRPoseHelper : MonoBehaviour
{
    [SerializeField] public Transform attachAnchor;
    [SerializeField] public GameObject rightHandPrefab;
    [SerializeField] public GameObject leftHandPrefab;
    [SerializeField] public GameObject rightSpherePrefab;
    [SerializeField] public GameObject leftSpherePrefab;
    [SerializeField] public bool hasTether = false;

    private LineRenderer lineRenderer = null;
    public GameObject activeModel;// for debug public
    private bool isTethered = false;

    private Settings settings => FindObjectOfType<Settings>();

    //private string currentPoseHand = "";

    private XRBaseInteractable interactable => GetComponent<XRBaseInteractable>();
    void Start()
    {
        if(!attachAnchor)
        {
            attachAnchor = transform;
        }
    }

    private void Update()
    {
        if(isTethered && lineRenderer)
        {
            Tether(lineRenderer, interactable.selectingInteractor.attachTransform.transform.position);
        }
    }

    private void OnEnable()
    {
        Settings.OnHandUpdate += SetCurrentModel;
        interactable.hoverEntered.AddListener(DisableHide);
        interactable.hoverExited.AddListener(ResetHide);
        interactable.selectExited.AddListener(ResetControllerSelectHide);
        interactable.selectEntered.AddListener(ForceModelShow);
    }

    private void OnDisable()
    {
        Settings.OnHandUpdate -= SetCurrentModel;
        interactable.hoverEntered.RemoveListener(DisableHide);
        interactable.hoverExited.RemoveListener(ResetHide);
        interactable.selectExited.RemoveListener(ResetControllerSelectHide);
        interactable.selectEntered.RemoveListener(ForceModelShow);
    }

    private void ForceModelShow(SelectEnterEventArgs args)
    {
        if (!args.interactor.TryGetComponent<XRDirectInteractor>(out XRDirectInteractor i))
        {
            return;
        }
        if (lineRenderer && hasTether && settings.GetCurrentHandModelSet() == HandModelSet.Sphere)
        {
            settings.GetBaseModelForHand(i).SetActive(true);
        }
    }

    private void DisableHide(HoverEnterEventArgs args)
    {
        if(!args.interactor.TryGetComponent<XRDirectInteractor>(out XRDirectInteractor i))
        {
            return;
        }
        lineRenderer = args.interactor.GetComponent<LineRenderer>();
        if (lineRenderer && hasTether && settings.GetCurrentHandModelSet() == HandModelSet.Sphere)
        {
            ((XRDirectInteractor)args.interactor).hideControllerOnSelect = false;
            if (args.interactor.GetComponent<ActionBasedController>().model)
            {
                args.interactor.GetComponent<ActionBasedController>().model.gameObject.SetActive(false);
                args.interactor.GetComponent<ActionBasedController>().model = settings.GetBaseModelForHand(i).transform;
                args.interactor.GetComponent<ActionBasedController>().model.gameObject.SetActive(true);
            }
        }
        else
        {
            ((XRDirectInteractor)args.interactor).hideControllerOnSelect = true;
        }
    }

    private void ResetHide(HoverExitEventArgs args)
    {
        if (!args.interactor.TryGetComponent<XRDirectInteractor>(out XRDirectInteractor i))
        {
            return;
        }
        if (lineRenderer && hasTether && settings.GetCurrentHandModelSet() == HandModelSet.Sphere && !interactable.isSelected)
        {
            ((XRDirectInteractor)args.interactor).hideControllerOnSelect = true;
            HidePose();
            if (lineRenderer.positionCount > 0)
            {
                DrawLineRenderer(lineRenderer, 0, attachAnchor.position);
                DrawLineRenderer(lineRenderer, 1, attachAnchor.position);
                lineRenderer.positionCount = 0;
            }
            isTethered = false;
            lineRenderer = null;
        }
    }

    private void ResetControllerSelectHide(SelectExitEventArgs args)
    {
        if (!args.interactor.TryGetComponent<XRDirectInteractor>(out XRDirectInteractor i))
        {
            return;
        }
        if (lineRenderer && hasTether && settings.GetCurrentHandModelSet() == HandModelSet.Sphere) 
        {
            ((XRDirectInteractor)args.interactor).hideControllerOnSelect = true;
            HidePose();
            isTethered = false;
            if (lineRenderer.positionCount > 0)
            {
                DrawLineRenderer(lineRenderer, 0, attachAnchor.position);
                DrawLineRenderer(lineRenderer, 1, attachAnchor.position);
                lineRenderer.positionCount = 0;
            }
            lineRenderer = null;
        }
    }

    private void SetCurrentModel(HandModelSet set)
    {
    }

    private GameObject GetPrefab(string hand)
    {
        switch(settings.GetCurrentHandModelSet())
        {
            case HandModelSet.Hand:
                return hand.Equals("LeftHand") ? leftHandPrefab : rightHandPrefab;
            case HandModelSet.Sphere:
                return hand.Equals("LeftHand") ? leftSpherePrefab : rightSpherePrefab;
            default:
                return null;
        }
    }

    public void ShowPose(XRBaseInteractor interactor)
    {
        if (activeModel)
        {
            HidePose();
        }
        GameObject model;
        if (interactor.CompareTag("LeftHand") || interactor.CompareTag("RightHand"))
        {
            model = GetPrefab(interactor.tag);
        }
        else
        {
            return;
        }

        if(!model)
        {
            return;
        }

        if (settings.GetCurrentHandModelSet() == HandModelSet.Sphere && hasTether)
        {
            //interactor.GetComponent<ActionBasedController>().model = null;
            lineRenderer = interactor.GetComponent<LineRenderer>();
            if (lineRenderer)
            {
                isTethered = true;
                ((XRDirectInteractor)interactor).hideControllerOnSelect = false;
                lineRenderer.positionCount = 2;
                DrawLineRenderer(lineRenderer, 0, attachAnchor.position);
                DrawLineRenderer(lineRenderer, 1, attachAnchor.position);
            }
            settings.EnableModelForHand((XRDirectInteractor)interactor);

        }
        else // what if i did both... like, have a sphere there AND tether
        // tried, only feels okay if the distance is far enough. overlapping is bad.
        {
            activeModel = Instantiate(model, attachAnchor);
            /*if (interactor.CompareTag("LeftHand"))
            {
                settings.isLPoseActive = true;
                currentPoseHand = "LeftHand";
            }
            else if (interactor.CompareTag("RightHand"))
            {
                settings.isRPoseActive = true;
                currentPoseHand = "RightHand";
            }*/
            settings.DisableModelForHand((XRDirectInteractor)interactor);
        }
    }

    private void Tether(LineRenderer renderer, Vector3 target)
    {
        DrawLineRenderer(renderer, 1, attachAnchor.position);
        DrawLineRenderer(renderer, 0, target);
    }

    private void DrawLineRenderer(LineRenderer renderer, int index, Vector3 targetPos)
    {
        if(renderer.positionCount > 0)
            renderer.SetPosition(index, targetPos);
    }

    public void HidePose()
    {
        if(activeModel)
            Destroy(activeModel);
        activeModel = null;
        if (lineRenderer)
        {
            DrawLineRenderer(lineRenderer, 0, attachAnchor.position);
            DrawLineRenderer(lineRenderer, 1, attachAnchor.position);
            lineRenderer.positionCount = 0;
        }
        /*if (currentPoseHand.Equals("LeftHand"))
            settings.isLPoseActive = false;
        else if (currentPoseHand.Equals("RightHand"))
            settings.isRPoseActive = false;*/
        //currentPoseHand = "";
        
    }
}
