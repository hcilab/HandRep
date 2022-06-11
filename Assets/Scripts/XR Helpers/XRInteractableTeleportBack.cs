using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInteractableTeleportBack : MonoBehaviour
{
    [SerializeField] float timeToTeleport = 30f;
    private Coroutine teleportRoutine = null;
    private XRBaseInteractable interactable => GetComponent<XRBaseInteractable>();
    [SerializeField] private Vector3 spawnPoint;
    private Quaternion spawnRotation;

    private void Awake()
    {
        if(spawnPoint == null)
        {
            spawnPoint = transform.position;
        }
        spawnRotation = transform.rotation;
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(NowSelected);
        interactable.selectExited.AddListener(Unselected);

    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(NowSelected);
        interactable.selectExited.RemoveListener(Unselected);
    }
    private void NowSelected(SelectEnterEventArgs args)
    {
        if (teleportRoutine != null)
        {
            StopCoroutine(teleportRoutine);
            teleportRoutine = null;
        }
    }

    private void Unselected(SelectExitEventArgs args)
    {
        teleportRoutine = StartCoroutine(KillObject());
    }

    private IEnumerator KillObject()
    {
        yield return new WaitForSeconds(timeToTeleport);
        transform.position = spawnPoint;
        transform.rotation = spawnRotation;
    }
}
