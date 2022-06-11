using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class T1_Manager : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int maxCount = 10; // for trigger area, not num to spawn, that is infinite
    [SerializeField] public int count = 0; // for trigger area, not num to spawn, that is infinite
    private GameObject spawnedItem = null;

    private Coroutine HideBellRoutine = null;//RespawnRoutine = null;
    // two trigger areas, left and right
    // each item, it switches which one to drop it into
    // when socket is empty, after 1 second put in a new one if one is not in it.
    //

    [SerializeField] private XRSocketInteractor scaleSocket;

    public UnityEvent<int, string> TrialComplete = null;
    public UnityEvent<int> TrialStart = null;

    [SerializeField] private GameObject scaleObj;
    [SerializeField] private GameObject endBell;
    [SerializeField] private GameObject startBell;

    private void Start()
    {
        if (endBell != null)
        {
            endBell.SetActive(false);
            startBell.SetActive(true);
        }
    }

    private TX_Manager manager => FindObjectOfType<TX_Manager>();

    /*IEnumerator ReplaceMushroom()
    {
        yield return new WaitForSeconds(0.15f);
        if (socket.gameObject.activeSelf && !socket.selectTarget && count < maxCount)
        {
            spawnedItem = null;
            spawnedItem = Instantiate(prefab);
            socket.interactionManager.ForceSelect(socket, spawnedItem.GetComponent<XRBaseInteractable>());
        }
        RespawnRoutine = null;
    }*/

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetTrial();
        }
    }

    public void ResetTrial()
    {
        if (HideBellRoutine != null)
        {
            StopCoroutine(HideBellRoutine);
            HideBellRoutine = null;
        }
        if (spawnedItem != null)
        {
            Destroy(spawnedItem);
        }
        if (socket.selectTarget)
        {
            Destroy(socket.selectTarget.gameObject);
        }
        spawnedItem = null;
        startBell.SetActive(true);
    }

    IEnumerator HideButton()
    {
        yield return new WaitForSeconds(1.15f);
        startBell.SetActive(false);
        HideBellRoutine = null;
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
    public void StartTask()
    {
        TrialStart.Invoke(count);
        if (socket.selectTarget)
        {
            Destroy(socket.selectTarget.gameObject);
        }
        spawnedItem = Instantiate(prefab, socket.attachTransform);
        if (!socket.selectTarget)
        {
            socket.interactionManager.ForceSelect(socket, spawnedItem.GetComponent<XRBaseInteractable>());
        }
        HideBellRoutine = StartCoroutine(HideButton());
    }

    public void RemovedFromScale(SelectExitEventArgs args)
    {
        Destroy(args.interactable.gameObject);
        spawnedItem = null;
        if (count < maxCount)
        {
            if (HideBellRoutine != null)
            {
                StopCoroutine(HideBellRoutine);
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
                scaleObj.SetActive(false);
                if (endBell != null)
                {
                    endBell.SetActive(true);
                }
                // spawn button to finish scene
            }
        }
    }

    public void AddedToScale(SelectEnterEventArgs args)
    {
        ObjectProperties props = args.interactable.GetComponent<ObjectProperties>();
        string w = props.GetProperty("weight");
        string weight = "none";
        if (w != null && !w.Equals(""))
            weight = float.Parse(props.GetProperty("weight")).ToString("F2");

        TrialComplete.Invoke(count, weight);
        count += 1;
    }
}
