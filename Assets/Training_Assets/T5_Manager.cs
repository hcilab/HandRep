using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class T5_Manager : MonoBehaviour
{
    [SerializeField] private TX_Manager txm;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject spawner;
    [SerializeField] private GameObject scale;
    [SerializeField] private GameObject dropoff;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject startbutton;
    [SerializeField] private GameObject prefab;
    [SerializeField] private XRSocketInteractor socket;
    private GameObject spawnedItem;
    public string colour;

    public UnityEvent UsedScale = null;
    public UnityEvent ColourChanged = null;
    public UnityEvent<int, string, string> TrialComplete = null;
    public UnityEvent<int, string> TrialStart = null;
    private Coroutine HideBellRoutine = null;
    public string firstColour = "none";
    //private int count = 0;
    //private int max = 3;
    public string[] order =
    {
        "Red", "Green", "Blue",
        "Yellow", "Purple"
       // "Red", "Green", "Blue",
       // "Yellow", "Orange", "Green",
       // "Purple", "Blue", "Red",
       // "Yellow"
    };

    private int index = 0;
    private void Awake()
    {
        text.text = "";
        button.SetActive(false);
        startbutton.SetActive(true);
    }

    public void StartTrial()
    {
        firstColour = "none";
        TrialStart.Invoke(index, order[index]);
        colour = order[index];
        text.text = colour;
        HideBellRoutine = StartCoroutine(HideButton());
        if (socket.selectTarget)
        {
            Destroy(socket.selectTarget.gameObject);
        }
        spawnedItem = Instantiate(prefab, socket.attachTransform);
        if (!socket.selectTarget)
        {
            socket.interactionManager.ForceSelect(socket, spawnedItem.GetComponent<XRBaseInteractable>());
        }
    }
    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetTrial();
        }
    }

    public void ResetTrial()
    {
        firstColour = "none";
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
        text.text = "";
        spawnedItem = null;
        startbutton.SetActive(true);
    }

    public void Submit(CubeColorController ccc)
    {
        TrialComplete.Invoke(index, ccc.GetColour().ToString(), firstColour);
        text.text = "";
        index += 1;
        bool done = false;
        if (index >= order.Length)
        {
            index = 0;
            done = txm.NextHand();
            if (done)
            {
                StartCoroutine(EndDelay());
                // done all hands
                //txm.SwitchScene();
            }
            else
            {
                StartCoroutine(PauseBreak());
            }
        }
        else
        {
            if (HideBellRoutine != null)
            {
                StopCoroutine(HideBellRoutine);
            }
            startbutton.SetActive(true);
        }
        if (!done)
        {
            colour = order[index];
            //text.text = colour;
        }
        firstColour = "none";
    }

    public bool Compare(CubeColorController.GemColour gc)
    {
        return gc.ToString().Equals(colour);
    }

    public void DestroyObject(HoverEnterEventArgs args)
    {
        CubeColorController ccc = args.interactable.GetComponent<CubeColorController>();
        Submit(ccc);
        if (args.interactable.gameObject)
        {
            Destroy(args.interactable.gameObject, 0.1f);
            spawnedItem = null;
        }
    }
    IEnumerator HideButton()
    {
        yield return new WaitForSeconds(1.15f);
        startbutton.SetActive(false);
        HideBellRoutine = null;
    }
    IEnumerator PauseBreak()
    {
        spawner.SetActive(false);
        scale.SetActive(false);
        dropoff.SetActive(false);
        startbutton.SetActive(false);
        yield return new WaitForSeconds(10f);
        spawner.SetActive(true);
        scale.SetActive(true);
        dropoff.SetActive(true);
        startbutton.SetActive(true);
    }

    IEnumerator EndDelay()
    {
        yield return new WaitForSeconds(0.2f);
        spawner.SetActive(false);
        scale.SetActive(false);
        startbutton.SetActive(false);
        dropoff.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        button.SetActive(true);
    }

    public void InScale(SelectEnterEventArgs args)
    {
        UsedScale.Invoke();
    }

    public void OnActivate()
    {
        ColourChanged.Invoke();
    }
}
