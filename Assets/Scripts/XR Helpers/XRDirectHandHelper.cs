using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRDirectInteractor))]
public class XRDirectHandHelper : MonoBehaviour
{

    XRDirectInteractor interactor => GetComponent<XRDirectInteractor>();
    Settings settings => FindObjectOfType<Settings>();

    [SerializeField] HighlightProfile highlightProfile;
    private List<HighlightEffect> highlightEffects = new List<HighlightEffect>();

    [SerializeField] Transform SphereAttach;
    [SerializeField] Transform IndexAttach;
    [SerializeField] Transform HandAttach;

    private void Awake()
    {
        interactor.hideControllerOnSelect = true;
        if(TryGetComponent<ActionBasedController>(out ActionBasedController controller))
        {
            controller.model = settings.GetBaseModelForHand(interactor).transform;
        }

    }

    private void OnEnable()
    {
        Settings.OnHandUpdate += SetCurrentModels;
        interactor.selectEntered.AddListener(HideHands);
        interactor.selectExited.AddListener(ShowHands);
        interactor.hoverEntered.AddListener(ReinforceHandActive);
        //interactor.hoverEntered.AddListener(HighlightHover);
        //interactor.hoverExited.AddListener(HighlightExit);
    }

    private void OnDisable()
    {
        Settings.OnHandUpdate -= SetCurrentModels;
        interactor.selectEntered.RemoveListener(HideHands);
        interactor.selectExited.RemoveListener(ShowHands);
        interactor.hoverEntered.RemoveListener(ReinforceHandActive);
        //interactor.hoverEntered.RemoveListener(HighlightHover);
        //interactor.hoverExited.RemoveListener(HighlightExit);
    }

    private void ReinforceHandActive(HoverEnterEventArgs args)
    {
        if (!args.interactable.TryGetComponent<XRPoseHelper>(out XRPoseHelper ph))
        {
            settings.EnableModelForHand((XRDirectInteractor)args.interactor);
        }
    }

    private void Update()
    {
        List<XRBaseInteractable> targets = new List<XRBaseInteractable>();
        interactor.GetValidTargets(targets);
        if (targets.Count > 0)
        {
            XRBaseInteractable primary = targets[0];
            if ( interactor.CanSelect(primary) &&
                interactor.selectTarget != primary &&
                    (!primary.isSelected ||
                    !primary.selectingInteractor.GetType().Equals(interactor.GetType()))
               &&
               !primary.TryGetComponent<HighlightEffect>(out HighlightEffect h))
            {
                // not selected, or selecting interactor is not a hand
                // does not have HLE
                RemoveAllOtherHighlights();
                HighlightEffect hle = AddHighlightEffect(primary.gameObject);
                highlightEffects.Add(hle);
            }
            else if(interactor.selectTarget != null)
            {
                RemoveAllOtherHighlights();
            }
        }
        else if (highlightEffects.Count > 0)
        {
            RemoveAllOtherHighlights();
        }
    }

    private void RemoveAllOtherHighlights()
    {
        if (highlightEffects.Count > 0)
        {
            foreach (HighlightEffect hle in highlightEffects)
            {
                Destroy(hle);
            }
            highlightEffects.Clear();
        }
    }

    private HighlightEffect AddHighlightEffect(GameObject obj)
    {
        HighlightEffect highlightEffect = obj.AddComponent<HighlightEffect>();
        highlightEffect.profile = highlightProfile;
        highlightEffect.ProfileLoad(highlightProfile);
        highlightEffect.highlighted = true;
        return highlightEffect;
    }


    private void DisableHoverHighlight(GameObject obj)
    {
        if(obj.TryGetComponent<HighlightEffect>(out HighlightEffect hle))
            Destroy(hle);
    }

    private void HideHands(SelectEnterEventArgs args)
    {
        DisableHoverHighlight(args.interactable.gameObject);
        XRDirectInteractor interactor = (XRDirectInteractor)args.interactor;
        if (TryGetComponent<ActionBasedController>(out ActionBasedController controller))
        {
            if(interactor.hideControllerOnSelect)
            {
                settings.DisableModelForHand(interactor);
                //controller.model = null;
            }
        }
    }

    private void ShowHands(SelectExitEventArgs args)
    {
        if (TryGetComponent<ActionBasedController>(out ActionBasedController controller))
        {
            //if(controller.model == null)
            //{
            //controller.model = null;
                settings.EnableModelForHand((XRDirectInteractor)args.interactor);
            //}
        }
        if (TryGetComponent<LineRenderer>(out LineRenderer lineRenderer) && lineRenderer.enabled == true
            && lineRenderer.positionCount > 0)
        {
            DrawLineRenderer(lineRenderer, 0, interactor.attachTransform.position);
            DrawLineRenderer(lineRenderer, 1, interactor.attachTransform.position);
            lineRenderer.positionCount = 0;
        }
    }
    private void DrawLineRenderer(LineRenderer renderer, int index, Vector3 targetPos)
    {
        if (renderer.positionCount > 0)
            renderer.SetPosition(index, targetPos);
    }

    private Transform FindAttach(HandModelSet set)
    {
        switch (set)
        {
            case HandModelSet.Index:
                return IndexAttach;
            case HandModelSet.Sphere:
                return SphereAttach;
            case HandModelSet.Hand:
                return HandAttach;
            case HandModelSet.Oculus:
                break;
            case HandModelSet.Wand:
                break;
            default:
                return null;
        }
        return null;
    }

    private void SetCurrentModels(HandModelSet set)
    {
        interactor.interactionManager.CancelInteractorSelection(interactor);
        if (TryGetComponent<ActionBasedController>(out ActionBasedController controller))
        {
            if(interactor)
            {
                //Transform attach = controller.model.Find(name = "attach");
                Transform attach = FindAttach(set);
                if (attach)
                {
                    interactor.attachTransform = attach;
                }
                else
                {
                    interactor.attachTransform = interactor.transform;
                }
            }
        }
    }
}
