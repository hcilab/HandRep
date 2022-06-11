using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ChestLidCloseDetector : MonoBehaviour
{
    [SerializeField] private ChestDropoff cdo;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip openingSnd;
    [SerializeField] private AudioClip closingSnd;
    private XRBaseInteractable containedItem;

    public UnityEvent OnClose = null;
    public UnityEvent OnCloseWithItem = null;
    public UnityEvent OnOpen = null;

    public bool OnCloseForgetObject = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ChestLid")){
            if (cdo.CheckIsClosed())
                audio.PlayOneShot(closingSnd);

            cdo.SetClosed(true);
            if(OnClose != null)
            {
                OnClose.Invoke();
            }
            if(OnCloseWithItem != null)
            {
                if(containedItem != null && containedItem.gameObject != null)
                {
                    OnCloseWithItem.Invoke();
                    if (OnCloseForgetObject)
                        RemoveItem();
                    //containedItem = null;
                }
            }
            //if(!audio.isPlaying)
            //    audio.PlayOneShot(closingSnd);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ChestLid"))
        {
            if (!cdo.CheckIsClosed())
                audio.PlayOneShot(openingSnd);
            cdo.SetClosed(false);
            if (OnOpen != null)
            {
                OnOpen.Invoke();
            }
            //if(!audio.isPlaying)
            //    audio.PlayOneShot(openingSnd);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("ChestLid"))
        {
            cdo.SetClosed(true);
        }
    }

    public void AddItem(HoverEnterEventArgs args)
    {
        containedItem = args.interactable;
    }

    public void RemoveItem()
    {
        containedItem = null;
    }
}
