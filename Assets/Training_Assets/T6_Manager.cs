using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class T6_Manager : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int maxCount = 10; // for trigger area, not num to spawn, that is infinite
    [SerializeField] public int count = 0; // for trigger area, not num to spawn, that is infinite
    private GameObject spawnedItem = null;

    public UnityEvent<int> TrialComplete = null;
    public UnityEvent<int> TrialStart = null;

    [SerializeField] private GameObject endBell;
    [SerializeField] private GameObject startBell;

    [SerializeField] XRBaseInteractable handle;
    [SerializeField] XRDrawer drawer;
    private Coroutine hideRoutine = null;

    private void Start()
    {
        if (endBell != null)
        {
            endBell.SetActive(false);
            startBell.SetActive(true);
        }
        handle.enabled = false;
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
        socket.gameObject.SetActive(false);
        startBell.SetActive(false);
        yield return new WaitForSeconds(10f);
        startBell.SetActive(true);
        socket.gameObject.SetActive(true);
    }

    public void ResetTrial()
    {

        if (spawnedItem != null)
        {
            Destroy(spawnedItem);
        }
        if (socket.selectTarget)
        {
            Destroy(socket.selectTarget.gameObject);
        }
        spawnedItem = null;
        if (hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            hideRoutine = null;
        }
        startBell.SetActive(true);
        drawer.ResetPosition();
    }

    public void StartTask()
    {
        drawer.ResetPosition();
        TrialStart.Invoke(count);
        handle.enabled = true;
        if (socket.selectTarget)
        {
            Destroy(socket.selectTarget.gameObject);
        }
        spawnedItem = Instantiate(prefab, socket.attachTransform);
        if (!socket.selectTarget)
        {
            socket.interactionManager.ForceSelect(socket, spawnedItem.GetComponent<XRBaseInteractable>());
        }
        hideRoutine = StartCoroutine(HideButton());
    }

    public void EnchantItem()
    {
        TrialComplete.Invoke(count);
        count += 1;
        Destroy(spawnedItem, 0.15f);
        //Destroy(args.interactable.gameObject);
        spawnedItem = null;
        handle.enabled = false;
        drawer.ResetPosition();
        if (count < maxCount)
        {
            if (hideRoutine != null)
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
                if (endBell != null)
                {
                    endBell.SetActive(true);
                }
            }
        }
    }
}
