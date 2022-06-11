using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDrawer : MonoBehaviour
{
    [Header("Interactions")]
    [SerializeField] private XRBaseInteractable handle = null;
    [SerializeField] private PhysicsMoveHelper mover = null;

    [Header("Movement/Direction")]
    [SerializeField] private Transform start = null;
    [SerializeField] private Transform end = null;

    private Vector3 grabPos = Vector3.zero;
    private float startPercent = 0.0f;
    private float currentPercent = 0.0f;

    public UnityEvent onMoveForward = null;
    public UnityEvent onMoveBackward = null;
    
    private enum State {
        None,
        MovingFW,
        MovingBW,
        Stationary
    }

    private State state = State.None;

    protected virtual void OnEnable()
    {
        handle.selectEntered.AddListener(StoreGrabInfo);
    }

    protected virtual void OnDisable()
    {
        handle.selectEntered.RemoveListener(StoreGrabInfo);
    }

    private void StoreGrabInfo(SelectEnterEventArgs args)
    {
        startPercent = currentPercent;
        grabPos = args.interactor.transform.position;
    }

    private void Update()
    {
        if(handle.isSelected)
        {
            UpdateDrawer();
        }
    }

    public void ResetPosition()
    {
        mover.MoveTo(start.position);
        currentPercent = 0.0f;
        startPercent = 0.0f;
    }

    private void UpdateDrawer()
    {
        float newPercent = startPercent + FindPercentDifference();
        mover.MoveTo(Vector3.Lerp(start.position, end.position, newPercent));

        float percent = Mathf.Clamp01(newPercent);

        if(percent > currentPercent)
        {
            if (!state.Equals(State.MovingBW) && onMoveBackward != null)
                onMoveBackward.Invoke();
            state = State.MovingBW;
            Debug.Log("Moving Back");
        }
        else if(percent < currentPercent)
        {
            if (!state.Equals(State.MovingFW) && onMoveForward != null)
                onMoveForward.Invoke();
            state = State.MovingFW;
            Debug.Log("Moving Forward");
        }
        else if(percent == currentPercent)
        {
            state = State.Stationary;
        }

        currentPercent = Mathf.Clamp01(percent);
    }

    private float FindPercentDifference()
    {
        float diff = 0.0f;
        Vector3 handPos = handle.selectingInteractor.transform.position;
        Vector3 pullDir = handPos - grabPos;
        Vector3 targetDir = end.position - start.position;

        float length = targetDir.magnitude;
        targetDir.Normalize();

        diff = Vector3.Dot(pullDir, targetDir) / length;

        return diff;
    }

    private void OnDrawGizmos()
    {
        if(start && end)
        {
            Gizmos.DrawLine(start.position, end.position);
        }
    }
}
