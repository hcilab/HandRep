using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRAnimPoseHelper : MonoBehaviour
{
    private Settings settings => FindObjectOfType<Settings>();
    private XRGrabInteractable interactable => GetComponent<XRGrabInteractable>();
    [SerializeField] string ingredient_type = "";
    [SerializeField] public Transform attachAnchor; 
    private void Start()
    {
        if (!attachAnchor)
        {
            attachAnchor = transform;
        }
    }

    private void OnEnable()
    {
        interactable.hoverEntered.AddListener(DisableHide);
        //interactable.hoverExited.AddListener(ResetHide);

        interactable.selectExited.AddListener(ExitObject);
        interactable.selectEntered.AddListener(EnterObject);
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(DisableHide);
        //interactable.hoverExited.RemoveListener(ResetHide);

        interactable.selectExited.RemoveListener(ExitObject);
        interactable.selectEntered.RemoveListener(EnterObject);
    }
    private void EnterObject(SelectEnterEventArgs args)
    {
        if (!args.interactor.TryGetComponent<XRDirectInteractor>(out XRDirectInteractor i))
        {
            return;
        }
        // we need to force hands to show again here
        if (settings.GetCurrentHandModelSet().Equals(HandModelSet.Hand)
            || settings.GetCurrentHandModelSet().Equals(HandModelSet.Sphere))
        {
            settings.EnableModelForHand((XRDirectInteractor)args.interactor);
        }
        else
        {
            settings.DisableModelForHand((XRDirectInteractor)args.interactor);
            //return;
        }

        if(settings.GetCurrentHandModelSet().Equals(HandModelSet.Hand))
        {
            Animator anim = args.interactor.GetComponentInChildren<IdleHandController>().gameObject.GetComponent<Animator>();
            foreach (ObjectProperty prop in GetComponent<ObjectProperties>().properties)
            {
                if(prop.key.Equals("ingredient_type"))
                {
                    // if it doesn't have the property it will fail gracefully
                    anim.SetBool(GetAnimBoolName(ingredient_type), CheckTypeEquals(prop.value, ingredient_type));
                    break;
                }
            }
        }
        else if (settings.GetCurrentHandModelSet().Equals(HandModelSet.Sphere))
        {
            Animator anim = args.interactor.GetComponentInChildren<InputHighlightSphereHand>().gameObject.GetComponent<Animator>();
            foreach (ObjectProperty prop in GetComponent<ObjectProperties>().properties)
            {
                if (prop.key.Equals("ingredient_type"))
                {
                    // if it doesn't have the property it will fail gracefully
                    anim.SetBool(GetAnimBoolName(ingredient_type), CheckTypeEquals(prop.value, ingredient_type));
                    if(prop.value.Equals("cork"))
                    {
                        anim.SetBool("holdingBottle", false);
                    }
                    break;
                }
            }
        }
        else if(settings.GetCurrentHandModelSet().Equals(HandModelSet.Index))
        {
            Animator anim = args.interactor.GetComponentInChildren<InputHighlightController>().gameObject.GetComponent<Animator>();
            foreach (ObjectProperty prop in GetComponent<ObjectProperties>().properties)
            {
                if (prop.key.Equals("ingredient_type"))
                {
                    // if it doesn't have the property it will fail gracefully
                    anim.SetBool(GetAnimBoolName(ingredient_type), CheckTypeEquals(prop.value, ingredient_type));
                    break;
                }
            }
        }
    }

    private bool CheckTypeEquals(string value, string value2)
    {
        string ingr_type = value.Replace("ench_", "");
        if (ingr_type.Contains("grnd_"))
        {
            ingr_type = "grnd_";
        }
        if (ingr_type.Contains("puzzle_cube"))
        {
            ingr_type = "puzzle_cube";
        }
        return ingr_type.Equals(value2);
    }

    private string GetAnimBoolName(string ingr_type)
    {
        if(ingr_type.Contains("grnd_"))
        {
            ingr_type = "grnd_";
        }
        if (ingr_type.Contains("puzzle_cube"))
        {
            ingr_type = "puzzle_cube";
        }
        // ground will have its own diff thing
        switch(ingr_type)
        {
            case "white_feather":
                return "holdingFeather";
            case "brown_mushroom":
                return "holdingBMushroom";
            case "bottle":
                return "holdingBottle";
            case "cork":
                return "holdingCork";
            case "puzzle_cube":
                return "holdingCube";
            case "grnd_":
                return "holdingDust";
            default:
                return null;
        }
    }

    private void DisableHide(HoverEnterEventArgs args)
    {
        if (!args.interactor.TryGetComponent<XRDirectInteractor>(out XRDirectInteractor i))
        {
            return;
        }
        else if(!settings.GetCurrentHandModelSet().Equals(HandModelSet.Hand)
            && !settings.GetCurrentHandModelSet().Equals(HandModelSet.Sphere))
        {
            ((XRDirectInteractor)args.interactor).hideControllerOnSelect = true;/////////////////////// was commented out?
        }
        else
        {
            ((XRDirectInteractor)args.interactor).hideControllerOnSelect = false;
        }
    }

    private void ResetHide(HoverExitEventArgs args)
    {
        if (!args.interactor.TryGetComponent<XRDirectInteractor>(out XRDirectInteractor i))
        {
            return;
        }
    }

    private void ExitObject(SelectExitEventArgs args)
    {
        if (!args.interactor.TryGetComponent<XRDirectInteractor>(out XRDirectInteractor i))
        {
            return;
        }

        settings.EnableModelForHand((XRDirectInteractor)args.interactor);
        if (settings.GetCurrentHandModelSet().Equals(HandModelSet.Hand))
        {
            Animator anim = args.interactor.GetComponentInChildren<IdleHandController>().gameObject.GetComponent<Animator>();
            foreach (ObjectProperty prop in GetComponent<ObjectProperties>().properties)
            {
                if (prop.key.Equals("ingredient_type"))
                {
                    anim.SetBool(GetAnimBoolName(ingredient_type), false);
                    break;
                }
            }
        }
        else if (settings.GetCurrentHandModelSet().Equals(HandModelSet.Sphere))
        {
            Animator anim = args.interactor.GetComponentInChildren<InputHighlightSphereHand>().gameObject.GetComponent<Animator>();
            foreach (ObjectProperty prop in GetComponent<ObjectProperties>().properties)
            {
                if (prop.key.Equals("ingredient_type"))
                {
                    anim.SetBool(GetAnimBoolName(ingredient_type), false);
                    break;
                }
            }
            anim.SetBool("holdingBottle", false);
            if(args.interactor.transform.Find("original_Attach") != null)
            {
                args.interactor.attachTransform.position = args.interactor.transform.Find("original_Attach").position;
            }
            
        }
        else if (settings.GetCurrentHandModelSet().Equals(HandModelSet.Index))
        {
            Animator anim = args.interactor.GetComponentInChildren<InputHighlightController>().gameObject.GetComponent<Animator>();
            foreach (ObjectProperty prop in GetComponent<ObjectProperties>().properties)
            {
                if (prop.key.Equals("ingredient_type"))
                {
                    // if it doesn't have the property it will fail gracefully
                    anim.SetBool(GetAnimBoolName(ingredient_type), false);
                    break;
                }
            }
        }
    }
}
