using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class T8BottleHelper : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor corkSocket;

    private void Start()
    {
        corkSocket.socketActive = false;
        corkSocket.enabled = false;
    }
    public void OnGrab(SelectEnterEventArgs args)
    {
        corkSocket.enabled = true;
        corkSocket.socketActive = true;
    }

    public void LetGo(SelectExitEventArgs args)
    {
        corkSocket.socketActive = false;
        corkSocket.enabled = false;
    }

    public void OnCork(SelectEnterEventArgs args)
    {
        T8_Manager t8m = FindObjectOfType<T8_Manager>();
        if(t8m != null)
        {
            t8m.CorkBottle(args);
        }
    }
}
