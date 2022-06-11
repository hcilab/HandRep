using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlankDialReceiver : MonoBehaviour, IDial
{

    [SerializeField] private TMP_Text valueText;

    [SerializeField] private float max = 360f;
    [SerializeField] private bool displayAsOtherNumberPercentage = false;
    [SerializeField] private float displayMax = 360f;

    public void DialUpdate(float value)
    {
        if (valueText)
        {
            if (displayAsOtherNumberPercentage)
            {
                float percent = value / max;
                valueText.text = ((int)(percent * displayMax)).ToString();
            }
            else
            {
                valueText.text = (value).ToString("f2");
            }
        }
    }

}
