using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRPropertySocket))]
public class ScaleWeightReceiver : MonoBehaviour
{
    [SerializeField] private TMP_Text valueText;
    private XRPropertySocket socket => GetComponent<XRPropertySocket>();
    [SerializeField] private string propertyName = "weight";

    private void Awake()
    {
        ClearText(null);
    }

    private void OnEnable()
    {
        socket.selectEntered.AddListener(SetText);
        socket.selectExited.AddListener(ClearText);
    }

    private void OnDisable()
    {
        socket.selectEntered.RemoveListener(SetText);
        socket.selectExited.RemoveListener(ClearText);
    }

    private void ClearText(SelectExitEventArgs args)
    {
        valueText.text = "";
    }

    private void SetText(SelectEnterEventArgs args)
    {
        ClearText(null);
        if(args.interactable.TryGetComponent<ObjectProperties>(out ObjectProperties properties))
        {
            List<ObjectProperty> props = properties.properties;
            string weight = props.Find((p) => p.key.Equals(propertyName)).value;
            bool isSolid = props.Find((p) => p.key.Equals("isSolid")).value.Equals("true");
            weight = isSolid ? float.Parse(weight).ToString("F2") + "g" : float.Parse(weight).ToString("F2") + "ml";
            string name = props.Find((p) => p.key.Equals("clean_name")).value;
            valueText.text = name + "\n" + weight;
        }
    }
}
