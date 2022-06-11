using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class T0_Manager : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int maxCount = 10; // for trigger area, not num to spawn, that is infinite
    [SerializeField] public int count = 0; // for trigger area, not num to spawn, that is infinite
    private GameObject spawnedItem = null;

    public UnityEvent<int> TrialComplete = null;
    public UnityEvent<int> TrialStart = null;


    [SerializeField] private GameObject DropArea;

    [SerializeField] private GameObject endBell;
    [SerializeField] private GameObject startBell;

    private Coroutine hideRoutine = null;

    private void Start()
    {
        //DropArea.SetActive(false); 
        if (endBell != null)
        {
            endBell.SetActive(false);
            startBell.SetActive(true);
        }
    }

    private TX_Manager manager => FindObjectOfType<TX_Manager>();

    /*private void ReplaceMushroom()
    {
        if(!socket.selectTarget && count < maxCount)
        {
            spawnedItem = null;
            spawnedItem = Instantiate(prefab);
            socket.interactionManager.ForceSelect(socket, spawnedItem.GetComponent<XRBaseInteractable>());
        }
    }*/

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetTrial();
        }
    }

    IEnumerator HideButton()
    {
        yield return new WaitForSeconds(1.15f);
        startBell.SetActive(false);
        hideRoutine = null;
    }

    IEnumerator PauseBreak()
    {
        //DropArea.SetActive(false);
        GameObject obj = null;
        if(socket.selectTarget)
        {
            obj = socket.selectTarget.gameObject;
            Destroy(obj);
        }
        socket.gameObject.SetActive(false);
        startBell.SetActive(false);
        yield return new WaitForSeconds(10f);
        //DropArea.SetActive(true);
        startBell.SetActive(true);
        socket.gameObject.SetActive(true);
    }

    public void ResetTrial()
    {
        if(hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }
        if(spawnedItem != null)
        {
            Destroy(spawnedItem);
        }
        if(socket.selectTarget)
        {
            Destroy(socket.selectTarget.gameObject);
        }
        spawnedItem = null;
        startBell.SetActive(true);
        /*spawnedItem = Instantiate(prefab, socket.attachTransform);
        if (!socket.selectTarget)
        {
            socket.interactionManager.ForceSelect(socket, spawnedItem.GetComponent<XRBaseInteractable>());
        }
        TrialStart.Invoke(count);
        */
    }

    public void StartTask()
    {
        TrialStart.Invoke(count);
        //DropArea.SetActive(true);
        if(socket.selectTarget)
        {
            Destroy(socket.selectTarget.gameObject);
        }
        spawnedItem = Instantiate(prefab, socket.attachTransform);
        if(!socket.selectTarget)
        {
            socket.interactionManager.ForceSelect(socket, spawnedItem.GetComponent<XRBaseInteractable>());
        }
        hideRoutine = StartCoroutine(HideButton());
    }

    public void DroppedItem(HoverEnterEventArgs args)
    {
        TrialComplete.Invoke(count);
        count += 1;
        Destroy(args.interactable.gameObject);
        spawnedItem = null;
        //DropArea.SetActive(false);
        if (count < maxCount)
        {
            if(hideRoutine != null)
            {
                StopCoroutine(hideRoutine);
            }
            startBell.SetActive(true);
        }
        else
        {
            bool isDone = manager.NextHand();
            if (!isDone)
            {
                StartCoroutine(PauseBreak());
                count = 0;
            }
            else
            {
                if (socket.selectTarget)
                {
                    XRBaseInteractable i = socket.selectTarget;
                    socket.allowSelect = false;
                    Destroy(i.gameObject);
                }
                socket.gameObject.SetActive(false);
                startBell.SetActive(false);
                DropArea.SetActive(false);
                if(endBell != null)
                {
                    endBell.SetActive(true);
                }
            }
        }
    }
}
