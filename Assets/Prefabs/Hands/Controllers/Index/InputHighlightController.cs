using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class InputHighlightController : MonoBehaviour
{
    [SerializeField] private InputActionReference controllerActionGrip;
    [SerializeField] private InputActionReference controllerActionTrigger;
    [SerializeField] private InputActionReference controllerActionAButton;
    [SerializeField] private InputActionReference controllerActionBButton;
    [SerializeField] private InputActionReference controllerActionThumbstick;
    [SerializeField] private InputActionReference controllerActionTrackPad;

    [SerializeField] Transform AButton;
    [SerializeField] Transform BButton;
    [SerializeField] Transform Grip;
    [SerializeField] Transform Thumbstick;
    [SerializeField] Transform Trackpad;
    [SerializeField] Transform Trigger;

    [SerializeField] Material baseMaterial;
    [SerializeField] Material selectMaterial;

    [SerializeField] XRDirectInteractor controller;
    [SerializeField] HandModelSet set = HandModelSet.Index;
    private Settings settings => FindObjectOfType<Settings>();
    private bool isHidden = false;

    private void Awake()
    {
        if(controller)
        {
            //controller.hideControllerOnSelect = true;
        }
    }
    private void OnEnable()
    {
        controllerActionGrip.action.performed += GripPressed;
        controllerActionGrip.action.canceled += GripUnpressed;

        controllerActionAButton.action.performed += APressed;
        controllerActionAButton.action.canceled += AUnpressed;

        controllerActionBButton.action.performed += BPressed;
        controllerActionBButton.action.canceled += BUnpressed;

        controllerActionTrigger.action.performed += TriggerPressed;
        controllerActionTrigger.action.canceled += TriggerUnpressed;

        controllerActionThumbstick.action.performed += ThumbMoved;
        controllerActionThumbstick.action.canceled += ThumbIdle;

        controllerActionTrackPad.action.performed += TrackMoved;
        controllerActionTrackPad.action.canceled += TrackIdle;

        if(controller != null)
        {
            controller.selectEntered.AddListener(this.EnforceDisappear);
            controller.selectExited.AddListener(this.EnforceReappear);
            controller.hoverEntered.AddListener(this.EnforceHideControllers);
        }

    }

    private void OnDisable()
    {
        controllerActionGrip.action.performed -= GripPressed; 
        controllerActionGrip.action.canceled -= GripUnpressed;

        controllerActionAButton.action.performed -= APressed;
        controllerActionAButton.action.canceled -= AUnpressed;

        controllerActionBButton.action.performed -= BPressed;
        controllerActionBButton.action.canceled -= BUnpressed;

        controllerActionTrigger.action.performed -= TriggerPressed;
        controllerActionTrigger.action.canceled -= TriggerUnpressed;

        controllerActionThumbstick.action.performed -= ThumbMoved;
        controllerActionThumbstick.action.canceled -= ThumbIdle;

        controllerActionTrackPad.action.performed -= TrackMoved;
        controllerActionTrackPad.action.canceled -= TrackIdle;
        
        if (controller != null)
        {
            controller.selectEntered.RemoveListener(this.EnforceDisappear);
            controller.selectExited.RemoveListener(this.EnforceReappear);
            controller.hoverEntered.RemoveListener(this.EnforceHideControllers);
        }
    }

    private void Update()
    {
        if(controller != null && controller.selectTarget != null &&
            !isHidden && settings.GetCurrentHandModelSet().Equals(set))
        {
            Disappear();
        }
        else if(controller != null && controller.selectTarget == null &&
            isHidden && settings.GetCurrentHandModelSet().Equals(set))
        {
            Reappear();
        }
    }

    private void EnforceDisappear(SelectEnterEventArgs args)
    {
        // this is not firing for some reason
        if (settings.GetCurrentHandModelSet().Equals(set))
        {
            settings.DisableModelForHand(controller);
            this.ToggleMeshVisibility(false);
            this.gameObject.SetActive(false);
            isHidden = true;
        }
    }
    private void EnforceReappear(SelectExitEventArgs args)
    {
        if (settings.GetCurrentHandModelSet().Equals(set))
        {
            this.gameObject.SetActive(true);
            this.ToggleMeshVisibility(true);
            settings.EnableModelForHand(controller);
            isHidden = false;
        }
    }

    private void Reappear()
    {
        this.gameObject.SetActive(true);
        this.ToggleMeshVisibility(true);
        settings.EnableModelForHand(controller);
        isHidden = false;
    }
    private void Disappear()
    {
        if (settings.GetCurrentHandModelSet().Equals(set))
        {
            settings.DisableModelForHand(controller);
            this.ToggleMeshVisibility(false);
            this.gameObject.SetActive(false);
            isHidden = true;
        }
    }

    private void EnforceHideControllers(BaseInteractionEventArgs args)
    {
        if (settings.GetCurrentHandModelSet().Equals(set))
        {
            //controller.hideControllerOnSelect = true;
        }
    }

    private void ChangeMaterial(Transform obj, Material mat)
    {
        obj.GetComponent<MeshRenderer>().material = mat;
    }

    private void GripPressed(InputAction.CallbackContext obj) => ChangeMaterial(Grip, selectMaterial);
    private void GripUnpressed(InputAction.CallbackContext obj) => ChangeMaterial(Grip, baseMaterial);


    private void APressed(InputAction.CallbackContext obj) => ChangeMaterial(AButton, selectMaterial);
    private void AUnpressed(InputAction.CallbackContext obj) => ChangeMaterial(AButton, baseMaterial);

    private void BPressed(InputAction.CallbackContext obj) => ChangeMaterial(BButton, selectMaterial);
    private void BUnpressed(InputAction.CallbackContext obj) => ChangeMaterial(BButton, baseMaterial);

    private void ThumbMoved(InputAction.CallbackContext obj) => ChangeMaterial(Thumbstick, selectMaterial);
    private void ThumbIdle(InputAction.CallbackContext obj) => ChangeMaterial(Thumbstick, baseMaterial);

    private void TrackMoved(InputAction.CallbackContext obj) => ChangeMaterial(Trackpad, selectMaterial);
    private void TrackIdle(InputAction.CallbackContext obj) => ChangeMaterial(Trackpad, baseMaterial);

    private void TriggerPressed(InputAction.CallbackContext obj) => ChangeMaterial(Trigger, selectMaterial);
    private void TriggerUnpressed(InputAction.CallbackContext obj) => ChangeMaterial(Trigger, baseMaterial);

    private void ToggleMeshVisibility(bool value)
    {
        this.GetComponent<MeshRenderer>().enabled = value;
        foreach(MeshRenderer mr in this.GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = value;
        }
    }

}
