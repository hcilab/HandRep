using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class T8_Manager : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private XRSocketInteractor socket2;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject prefab2;
    [SerializeField] private int maxCount = 10; // for trigger area, not num to spawn, that is infinite
    [SerializeField] public int count = 0; // for trigger area, not num to spawn, that is infinite
    private GameObject spawnedItem = null;
    private GameObject spawnedItem2 = null;

    public UnityEvent<int> TrialComplete = null;
    public UnityEvent<int> TrialStart = null;


    [SerializeField] private GameObject endBell;
    [SerializeField] private GameObject startBell;

    private Coroutine hideRoutine = null;

    private void Start()
    {
        if (endBell != null)
        {
            endBell.SetActive(false);
            startBell.SetActive(true);
        }
    }

    private TX_Manager manager => FindObjectOfType<TX_Manager>();

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
        GameObject obj = null;
        if (socket.selectTarget)
        {
            obj = socket.selectTarget.gameObject;
            Destroy(obj);
        }
        if(socket2.selectTarget)
        {            
            Destroy(socket2.selectTarget.gameObject);
        }
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }
        socket2.gameObject.SetActive(false);
        socket.gameObject.SetActive(false);
        startBell.SetActive(false);
        yield return new WaitForSeconds(10f);
        socket.gameObject.SetActive(true);
        socket2.gameObject.SetActive(true);
        startBell.SetActive(true);
    }

    public void ResetTrial()
    {

        if (spawnedItem != null)
        {
            Destroy(spawnedItem);
        }
        if(spawnedItem2 != null)
        {
            Destroy(spawnedItem2);
        }
        if (socket.selectTarget)
        {
            Destroy(socket.selectTarget.gameObject);
        }
        if (socket2.selectTarget)
        {
            Destroy(socket2.selectTarget.gameObject);
        }
        spawnedItem = null;
        spawnedItem2 = null;
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }
        startBell.SetActive(true);
    }

    public void StartTask()
    {
        TrialStart.Invoke(count);
        if (socket.selectTarget)
        {
            Destroy(socket.selectTarget.gameObject);
        }
        if (socket2.selectTarget)
        {
            Destroy(socket2.selectTarget.gameObject);
        }
        spawnedItem = Instantiate(prefab, socket.attachTransform);
        spawnedItem2 = Instantiate(prefab2, socket2.attachTransform);
        if (!socket.selectTarget)
        {
            socket.interactionManager.ForceSelect(socket, spawnedItem.GetComponent<XRBaseInteractable>());
        }
        if (!socket2.selectTarget)
        {
            socket2.interactionManager.ForceSelect(socket2, spawnedItem2.GetComponent<XRBaseInteractable>());
        }
        hideRoutine = StartCoroutine(HideButton());
    }

    public void CorkBottle(SelectEnterEventArgs args)
    {
        TrialComplete.Invoke(count);
        count += 1;
        //if(spawnedItem.TryGetComponent<XRBaseInteractable>(out XRBaseInteractable inter))
        //{
            //if(inter.isSelected)
           //     inter.interactionManager.SelectCancel(inter.selectingInteractor, inter);
           // inter.enabled = false;
        //}


        if (count < maxCount)
        {
            Destroy(spawnedItem2, 0.21f);
            Destroy(spawnedItem, 0.25f);
            spawnedItem2 = null;
            spawnedItem = null;
            if (hideRoutine != null)
            {
                StopCoroutine(hideRoutine);
            }
            startBell.SetActive(true);
        }
        else
        {
            Destroy(spawnedItem2);
            Destroy(spawnedItem, 0.25f);
            spawnedItem.GetComponent<XRBaseInteractable>().enabled = false;
            spawnedItem2 = null;
            spawnedItem = null;
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
                if (socket2.selectTarget)
                {
                    XRBaseInteractable i = socket2.selectTarget;
                    socket2.allowSelect = false;
                    Destroy(i.gameObject);
                }
                socket2.gameObject.SetActive(false);
                startBell.SetActive(false);
                if (endBell != null)
                {
                    endBell.SetActive(true);
                }
            }
        }
    }
}
