using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T9CorkHelper : MonoBehaviour
{

    [SerializeField] XRGrabInteractable cork;
    [SerializeField] Rigidbody corkRB;
    [SerializeField] XRSocketInteractor BottleCorkSocket;

    bool firstPickup = true;
    public void Awake()
    {
        BottleCorkSocket.socketActive = false;
        corkRB.useGravity = false;
        corkRB.isKinematic = true;
        cork.enabled = false;
    }

    public void OnPickUp(SelectEnterEventArgs args)
    {
        BottleCorkSocket.socketActive = true;
        corkRB.isKinematic = false;
        corkRB.useGravity = true;
        cork.enabled = true;
        if(firstPickup)
            BottleCorkSocket.interactionManager.ForceSelect(BottleCorkSocket, cork);
        firstPickup = false;
    }

    public void OnUncork(SelectExitEventArgs args)
    {
        T9_Manager t9m = FindObjectOfType<T9_Manager>();
        if(t9m != null)
        {
            t9m.UncorkedBottle(args.interactable.gameObject);
        }
    }

}
