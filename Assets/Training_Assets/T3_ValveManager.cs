using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class T3_ValveManager : MonoBehaviour
{
    [SerializeField] private XRBaseInteractable handle;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject endbutton;
    [SerializeField] private TMP_Text prompt;
    [SerializeField] private T3DataLog log;
    [SerializeField] DialReceiver dr;
    public UnityEvent<int, int, int, int> TrialComplete = null;
    public UnityEvent<int> TrialStart = null;

    public bool Started = false;

    private TX_Manager txm => FindObjectOfType<TX_Manager>();
    public int max = -1;
    public int min = -1;
    private Coroutine buttonHide = null;

    // Discuss with scott if these should be the same or diff ranges for each hand rep.
    static int[][] order1 = {
     new int[] {30, 36},
     new int[] {76, 82},
     //new int[] {21, 27},
     new int[] {50, 56},
     new int[] {87, 93},
   //  new int[] {60, 66},
   //  new int[] {18, 24},
   //  new int[] {44, 50},
   //  new int[] {76, 82},
   //  new int[] {38, 44},
     new int[] {0, 0},
    };

    static int[][] order2 = {
     new int[] {30, 36},
     new int[] {76, 82},
     //new int[] {21, 27},
     new int[] {50, 56},
     new int[] {87, 93},
   //  new int[] {60, 66},
   //  new int[] {18, 24},
   //  new int[] {44, 50},
   //  new int[] {76, 82},
   //  new int[] {38, 44},
     new int[] {0, 0},
    };

    static int[][] order3 = {
     new int[] {30, 36},
     new int[] {76, 82},
     //new int[] {21, 27},
     new int[] {50, 56},
     new int[] {87, 93},
   //  new int[] {60, 66},
   //  new int[] {18, 24},
   //  new int[] {44, 50},
   //  new int[] {76, 82},
   //  new int[] {38, 44},
     new int[] {0, 0},
    };

    /*static int[][] order2 = {
     new int[] {44, 50},
     new int[] {94, 100},
     new int[] {66, 72},
     new int[] {33, 39},
     new int[] {52, 58},
     new int[] {0, 0},
    };

    static int[][] order3 = {
     new int[] {42, 48},
     new int[] {66, 72},
     new int[] {33, 38},
     new int[] {90, 96},
     new int[] {38, 44},
     new int[] {0, 0},
    };
    */
    static int[][][] orders = { order1, order2, order3 };
    int index = 0;
    int internalIndex = 0;
    int[][] currentOrder;
    public void Awake()
    {
        prompt.text = "";
        currentOrder = orders[index];
        button.SetActive(true);
        endbutton.SetActive(false);
    }
    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetTrial();
        }
    }

    private void ResetTrial()
    {
        Started = false;
        handle.enabled = false;
        prompt.text = "";
        ShowButton(null);
    }

    private void FinishOrder()
    {
        index++;
        if(index >= orders.Length)
        {
            // done
            //txm.SwitchScene();
            button.SetActive(false);
            endbutton.SetActive(true);
            handle.enabled = false;
        }
        else
        {
            currentOrder = orders[index];
            internalIndex = 0;
            txm.NextHand();
        }
    }

    public void SubmitValue() // button press
    {
        if (!Started)
        {
            TrialStart.Invoke(internalIndex);
            handle.enabled = true;
            Started = true;
            min = currentOrder[internalIndex][0];
            max = currentOrder[internalIndex][1];
            prompt.text = min.ToString() + " - " + max.ToString();
            buttonHide = StartCoroutine(HideButton());
        }
        else
        {
            Started = false;
            log.LogSubmitEvent();
            handle.enabled = false;
            TrialComplete.Invoke(internalIndex, min, max, int.Parse(dr.GetCurrentValueString()));
            internalIndex += 1;
            if (internalIndex >= currentOrder.Length)
            {
                FinishOrder();
            }
            prompt.text = "";
            /*if (index < orders.Length)
            {
                min = currentOrder[internalIndex][0];
                max = currentOrder[internalIndex][1];
                prompt.text = min.ToString() + " - " + max.ToString();
                buttonHide = StartCoroutine(HideButton());
            }*/
        }
    }

    IEnumerator HideButton()
    {
        yield return new WaitForSeconds(1.1f);
        button.SetActive(false);
        buttonHide = null;
    }

    public bool Compare(int val)
    {
        return val >= min && val <= max;
    }

    public bool Compare(string val)
    {
        return Compare(int.Parse(val));
    }

    public void ShowButton(SelectExitEventArgs args)
    {
        if (!button.activeSelf)
        {
            button.SetActive(true);
        }
        if(button.TryGetComponent<AudioSource>(out AudioSource asource)) {
            if(asource.isPlaying)
                asource.Stop();
        }
    }
}
