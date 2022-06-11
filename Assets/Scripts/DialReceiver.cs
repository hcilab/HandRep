using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(XRGrabRotatable))]
public class DialReceiver : MonoBehaviour, IDial
{

    [SerializeField] private TMP_Text valueText;

    [SerializeField] private float max = 360f;
    [SerializeField] private bool displayAsOtherNumberPercentage = false;
    [SerializeField] private float displayMax = 360f;
    public UnityEvent<float, float> OnRotateUpdate = null;

    public void DialUpdate(float value)
    {
        if (valueText)
        {
            if(displayAsOtherNumberPercentage)
            {
                float percent = value / max;
                valueText.text = ((int)(percent * displayMax)).ToString();
            }
            else
            {
                valueText.text = (value).ToString("f2");
            }
        }
        if(OnRotateUpdate != null)
        {
            OnRotateUpdate.Invoke(value, GetCurrentValue());
        }
    }

    public float GetCurrentValue()
    {
        return float.Parse(valueText.text);
    }

    public string GetCurrentValueString()
    {
        return valueText.text;
    }

}
