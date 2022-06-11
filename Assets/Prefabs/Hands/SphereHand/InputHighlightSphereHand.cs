using UnityEngine;
using UnityEngine.InputSystem;


public class InputHighlightSphereHand : MonoBehaviour
{
    [SerializeField] private InputActionReference leftControllerActionGrip;
    [SerializeField] private InputActionReference rightControllerActionGrip;

    [SerializeField] Transform Grip;

    private void Awake()
    {
        Grip.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnEnable()
    {
        if(transform.parent.CompareTag("RightHand"))
        {
            rightControllerActionGrip.action.performed += GripPressed;
            rightControllerActionGrip.action.canceled += GripUnpressed;
        }
        else if (transform.parent.CompareTag("LeftHand"))
        {
            leftControllerActionGrip.action.performed += GripPressed;
            leftControllerActionGrip.action.canceled += GripUnpressed;
        }
    }

    private void OnDisable()
    {
        if (transform.parent.CompareTag("RightHand"))
        {
            rightControllerActionGrip.action.performed -= GripPressed;
            rightControllerActionGrip.action.canceled -= GripUnpressed;
        }
        else if (transform.parent.CompareTag("LeftHand"))
        {
            leftControllerActionGrip.action.performed -= GripPressed;
            leftControllerActionGrip.action.canceled -= GripUnpressed;
        }

    }


    private void GripPressed(InputAction.CallbackContext obj) => Grip.GetComponent<MeshRenderer>().enabled = true;
    private void GripUnpressed(InputAction.CallbackContext obj) => Grip.GetComponent<MeshRenderer>().enabled = false;
}
