using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRBaseInteractable))]
public class XRInteractableDestroyer : MonoBehaviour
{

    [SerializeField] float timeToKill = 30f;
    private Coroutine deathRoutine = null;
    private XRBaseInteractable interactable => GetComponent<XRBaseInteractable>();
    //private InteractableLogger elog => GetComponent<InteractableLogger>();
    [SerializeField] GameObject[] additionalObjects = { };
    public UnityEvent onTimeout = null;

    private void Awake()
    {
        if(!gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(NowSelected);
        interactable.selectExited.AddListener(Unselected);

    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(NowSelected);
        interactable.selectExited.RemoveListener(Unselected);
        if(deathRoutine != null)
        {
            StopCoroutine(deathRoutine);
            deathRoutine = null;
        }
    }
    public void NowSelected(SelectEnterEventArgs args)
    {
        if(deathRoutine != null)
        {
            StopCoroutine(deathRoutine);
            deathRoutine = null;
        }
    }

    public void Unselected(SelectExitEventArgs args)
    {
        if(gameObject.activeSelf && gameObject.activeInHierarchy)
            deathRoutine = StartCoroutine(KillObject());
    }

    private IEnumerator KillObject()
    {
        yield return new WaitForSeconds(timeToKill);
        foreach (GameObject go in additionalObjects)
        {
            Destroy(go);
        }
        if (onTimeout != null)
            onTimeout.Invoke();
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        /*if(elog)
        {
            elog.LogDestroyEvent("timeout");
        }*/
        if(deathRoutine != null)
        {
            StopCoroutine(deathRoutine);
            deathRoutine = null;
            //Destroy(this.gameObject);
        }
    }

}


