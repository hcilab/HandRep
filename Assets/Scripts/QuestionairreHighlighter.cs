using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionairreHighlighter : MonoBehaviour
{

    [SerializeField] Color unselectedColour = Color.white;
    [SerializeField] Color selectedColour = Color.cyan;

    [SerializeField] TMP_Text[] labels = { };

    [SerializeField] Slider slider;
    private int index = 4;

    private void Awake()
    {
        foreach(TMP_Text text in labels)
        {
            text.color = unselectedColour;
        }
        
        if (slider)
        {
            labels[(int)slider.value].color = selectedColour;
            index = (int)slider.value;
        }
    }

    public void NewSelection(float value)
    {
        labels[index].color = unselectedColour;
        index = (int)value;
        labels[index].color = selectedColour;
    }
}
