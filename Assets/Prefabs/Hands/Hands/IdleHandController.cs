using UnityEngine;
using UnityEngine.InputSystem;

public class IdleHandController : MonoBehaviour
{
    [SerializeField] private InputActionReference controllerActionGrip;

    private Animator handAnimator => GetComponent<Animator>();

    private void OnEnable()
    {
        controllerActionGrip.action.performed += GripPressed;
    }

    private void OnDisable()
    {
        controllerActionGrip.action.performed -= GripPressed;
    }

    private void GripPressed(InputAction.CallbackContext obj)
    {
        handAnimator.SetFloat("Grip", obj.ReadValue<float>());
    }
}
