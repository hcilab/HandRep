using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class T2_Manager : MonoBehaviour
{
    [SerializeField] private int maxCount = 10; // for trigger area, not num to spawn, that is infinite
    [SerializeField] public int count = 0; // for trigger area, not num to spawn, that is infinite

    private Coroutine HideBellRoutine = null;


    public UnityEvent<int> TrialComplete = null;
    public UnityEvent<int> TrialStart = null;

    [SerializeField] private GameObject lever;
    [SerializeField] private XRBaseInteractable handle;
    [SerializeField] private GameObject endBell;
    [SerializeField] private GameObject startBell;

    private void Start()
    {
        if (endBell != null)
        {
            endBell.SetActive(false);
            startBell.SetActive(true);
            handle.enabled = false;
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
        handle.enabled = false;
        if (HideBellRoutine != null)
        {
            StopCoroutine(HideBellRoutine);
            HideBellRoutine = null;
        }
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

        handle.enabled = false;
        startBell.SetActive(false);
        yield return new WaitForSeconds(10f);
        startBell.SetActive(true);
    }
    public void StartTask()
    {
        TrialStart.Invoke(count);
        handle.enabled = true;
        HideBellRoutine = StartCoroutine(HideButton());
    }

    public void PulledDown()
    {
        TrialComplete.Invoke(count);
        count += 1;
        if (count < maxCount)
        {
            if (HideBellRoutine != null)
            {
                StopCoroutine(HideBellRoutine);
            }
            startBell.SetActive(true);
            handle.enabled = false;
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
                handle.enabled = false;
                startBell.SetActive(false);
                //lever.SetActive(false);
                if (endBell != null)
                {
                    endBell.SetActive(true);
                }
                // spawn button to finish scene
            }
        }
    }
}
