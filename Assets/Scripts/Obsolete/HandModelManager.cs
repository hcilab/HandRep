using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[Obsolete("This class is obsolete and replaced by the XR Pose Helper.", true)]
public class HandModelManager : MonoBehaviour
{
    // todo: pose for hammer, and then make hammer align properly in hand
    public GameObject RightHandModel;
    public GameObject LeftHandModel;

    public GameObject RightIndexModel;
    public GameObject LeftIndexModel;

    public GameObject RightOculusModel;
    public GameObject LeftOculusModel;

    public GameObject RightWandModel;
    public GameObject LeftWandModel;

    public GameObject RightSphereModel;
    public GameObject LeftSphereModel;

    public List<HandModelSet> controllerSets = new List<HandModelSet>{ HandModelSet.Index, HandModelSet.Oculus, HandModelSet.Wand };
    //public List<HandModelSet> hideOnSelect = new List<HandModelSet> { HandModelSet.Index, HandModelSet.Oculus, HandModelSet.Wand }; // not needed?
    public List<HandModelSet> replaceHandsOnSelect = new List<HandModelSet> { HandModelSet.Hand, HandModelSet.Sphere };

    [SerializeField] private GameObject currentRightModel;
    [SerializeField] private GameObject currentLeftModel;
    public HandModelSet currentSet { get; private set; }


    private void OnEnable()
    {
        Settings.OnHandUpdate += SetCurrentModels;
    }

    private void OnDisable()
    {
        Settings.OnHandUpdate -= SetCurrentModels;
    }

    private void SetCurrentModels(HandModelSet set)
    {
        switch (set)
        {
            case HandModelSet.Index:
                currentRightModel = RightIndexModel;
                currentLeftModel = LeftIndexModel;
                break;
            case HandModelSet.Oculus:
                currentRightModel = RightOculusModel;
                currentLeftModel = LeftOculusModel;
                break;
            case HandModelSet.Wand:
                currentRightModel = RightWandModel;
                currentLeftModel = LeftWandModel;
                break;
            case HandModelSet.Hand:
                currentRightModel = RightHandModel;
                currentLeftModel = LeftHandModel;
                break;
            case HandModelSet.Sphere:
                currentRightModel = RightSphereModel;
                currentLeftModel = LeftSphereModel;
                break;
            default:
                currentRightModel = RightSphereModel;
                currentLeftModel = LeftSphereModel;
                break;
        }
        if (!currentRightModel)
        {
            currentRightModel = RightSphereModel;
        }
        if (!currentLeftModel)
        {
            currentLeftModel = LeftSphereModel;
        }

        currentSet = set;
    }

    public void EnableCurrentModel(XRBaseInteractor interactor)
    {
        DisableAll();

        if(!replaceHandsOnSelect.Contains(currentSet))
        {
            // We do not want to show a hand model
            return;
        }

        if (interactor.CompareTag("LeftHand"))
        {
            currentLeftModel.SetActive(true);
        }
        else if (interactor.CompareTag("RightHand"))
        {
            currentRightModel.SetActive(true);
        }
    }

    public void DisableCurrentModel(XRBaseInteractor interactor)
    {
        if (interactor.CompareTag("LeftHand"))
        {
            currentLeftModel.SetActive(false);
        }
        else if (interactor.CompareTag("RightHand"))
        {
            currentRightModel.SetActive(false);
        }
    }

    public void DisableAll()
    {
        if(RightHandModel)
            RightHandModel.SetActive(false);
        if (LeftHandModel) 
            LeftHandModel.SetActive(false);
        if (RightIndexModel)
            RightIndexModel.SetActive(false);
        if (LeftIndexModel)
            LeftIndexModel.SetActive(false);
        if(RightOculusModel)
            RightOculusModel.SetActive(false);
        if (LeftOculusModel)
            LeftOculusModel.SetActive(false);
        if (RightWandModel)
            RightWandModel.SetActive(false);
        if (LeftWandModel)
            LeftWandModel .SetActive(false);
        if (RightSphereModel)
            RightSphereModel.SetActive(false);
        if (LeftSphereModel)
            LeftSphereModel .SetActive(false);
    }
}
