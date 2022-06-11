using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class T10_Manager : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int maxCount = 10; // for trigger area, not num to spawn, that is infinite
    [SerializeField] public int count = 0; // for trigger area, not num to spawn, that is infinite
    private GameObject spawnedItem = null;

    public UnityEvent<int, float, float, float> TrialComplete = null;
    public UnityEvent<int> TrialStart = null;

    [SerializeField] private GameObject endBell;
    [SerializeField] private GameObject startBell;
    [SerializeField] private GameObject submitBell;

    public float min = 5.00f;
    public float max = 6.00f;

    [SerializeField] T10SummaryLog summary;
    [SerializeField] AlchemyRecipe cauldron;
    private Coroutine hideRoutine = null;

    [SerializeField] TMP_Text text;


    private void Start()
    {
        if (endBell != null)
        {
            endBell.SetActive(false);
            startBell.SetActive(true);
            submitBell.SetActive(false);
        }
        text.text = "Get to as close to the minimum as possible\n";
        text.text += "Min: " + min.ToString("f2");
        text.text += "\tMax: " + max.ToString("f2");
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

    IEnumerator HideSubmitButton()
    {
        yield return new WaitForSeconds(1f);
        submitBell.SetActive(false);
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
        submitBell.SetActive(false);
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
        cauldron.ClearList();
        summary.exceeded = false;
    }

    public void StartTask()
    {
        summary.exceeded = false;
        submitBell.SetActive(false);
        text.text = "Get to as close to the minimum as possible\n";
        text.text += "Min: " + min.ToString("f2");
        text.text += "\tMax: " + max.ToString("f2");
        cauldron.ClearList();
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
        hideRoutine = StartCoroutine(HideButton());
    }

    public void OnIngredientAdd(Ingredient ix, Dictionary<string, Ingredient> ingredients)
    {
        float val = 0.00f;
        submitBell.SetActive(true);
        foreach (Ingredient i in cauldron.GetIngredients().Values)
        {
            if (i.key.Equals("red_bottle"))
            {
                val = i.quantity;
            }
        }

        if(val >= max)
        {
            Exceeded();
        }
    }

    public void Exceeded()
    {
        T10PourHelper ph = spawnedItem.GetComponent<T10PourHelper>();
        ph.DisablePouring();
        summary.exceeded = true;
    }

    public void OnSubmit()
    {
        StartCoroutine(HideSubmitButton());
        float val = 0.0f;
        foreach (Ingredient i in cauldron.GetIngredients().Values)
        {
            if (i.key.Equals("red_bottle"))
            {
                val = i.quantity;
            }
        }
        TrialComplete.Invoke(count, min, max, val);
        count += 1;
        //Destroy(spawnedItem, 0.5f);
        if(spawnedItem != null)
            Destroy(spawnedItem);
        spawnedItem = null;
        cauldron.ClearList();
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
