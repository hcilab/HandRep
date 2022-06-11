using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class T4_Manager : MonoBehaviour
{
    [SerializeField] private int maxCount = 10; // for trigger area, not num to spawn, that is infinite
    [SerializeField] public int count = 0; // for trigger area, not num to spawn, that is infinite

    private Coroutine HideBellRoutine = null;


    public UnityEvent<int, bool> TrialComplete = null;
    public UnityEvent<int> TrialStart = null;

    [SerializeField] private GameObject Chest;
    [SerializeField] private XRBaseInteractable lid;
    [SerializeField] private GameObject endBell;
    [SerializeField] private GameObject startBell;
    [SerializeField] private GameObject prefab;
    [SerializeField] private XRSocketInteractor socket;
    private GameObject spawnedItem = null;

    private void Start()
    {
        if (endBell != null)
        {
            endBell.SetActive(false);
            startBell.SetActive(true);
            lid.enabled = false;
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
        if (HideBellRoutine != null)
        {
            StopCoroutine(HideBellRoutine);
            HideBellRoutine = null;
        }
        spawnedItem = null;
        lid.enabled = false;
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
        lid.enabled = false;
        startBell.SetActive(false);
        yield return new WaitForSeconds(10f);
        socket.gameObject.SetActive(true);
        startBell.SetActive(true);
    }
    public void StartTask()
    {
        TrialStart.Invoke(count);
        lid.enabled = true; 
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

    public void ClosedWithItem()
    {
        TrialComplete.Invoke(count, lid.isSelected);
        Destroy(spawnedItem, 0.2f);
        spawnedItem = null;
        count += 1;
        if (count < maxCount)
        {
            if (HideBellRoutine != null)
            {
                StopCoroutine(HideBellRoutine);
            }
            startBell.SetActive(true);
            lid.enabled = false;
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
                startBell.SetActive(false);
                lid.enabled = false;
                //Chest.SetActive(false);
                if (endBell != null)
                {
                    endBell.SetActive(true);
                }
                // spawn button to finish scene
            }
        }
    }
}
