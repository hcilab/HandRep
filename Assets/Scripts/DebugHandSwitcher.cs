using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(DebugDataLog))]
public class DebugHandSwitcher : MonoBehaviour
{
    public bool sourceMustBeController = false;

    [SerializeField] HandModelSet set;

    private void ChangeHandSet()
    {
        Settings settings = FindObjectOfType<Settings>();

        if (settings.GetCurrentHandModelSet() != set)
        {
            settings.SetHandModelSet(set);
            if(TryGetComponent<DebugDataLog>(out DebugDataLog ddl)) {
                ddl.LogEvent();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GetController(other);
    }


    private void GetController(Collider other)
    {
        if (other.gameObject.TryGetComponent<XRBaseController>(out XRBaseController controller))
        {
            ChangeHandSet();
        }
    }
}
