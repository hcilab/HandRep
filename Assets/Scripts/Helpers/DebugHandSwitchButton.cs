using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugHandSwitchButton : MonoBehaviour
{
    [SerializeField] private InputActionReference LeftSecondaryButton;
    private int index = 0;
    HandModelSet[] sets = { HandModelSet.Sphere, HandModelSet.Index, HandModelSet.Hand };

    private void Awake()
    {

    }

    private void OnEnable()
    {
        LeftSecondaryButton.action.performed += ChangeHandSet;
    }

    private void OnDisable()
    {
        LeftSecondaryButton.action.performed -= ChangeHandSet;
    }

    private void ChangeHandSet(InputAction.CallbackContext ctx)
    {
        Settings settings = FindObjectOfType<Settings>();
        while (settings.GetCurrentHandModelSet().Equals(sets[index]))
        {
            index = Random.Range(0, sets.Length);
        }

        settings.SetHandModelSet(sets[index]);
    }
}
