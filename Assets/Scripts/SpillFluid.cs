using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ParticleSystem))]
public class SpillFluid : MonoBehaviour
{
    private ParticleSystem particles => GetComponent<ParticleSystem>();
    [SerializeField] private XRSocketInteractor corkSocket;
    [SerializeField] private XRBaseInteractable bottle;
    [SerializeField] private bool OnlyPourWhenHeld = false;

    //[SerializeField] float threshold = 45f;

    public UnityEvent OnPour = null;
    public UnityEvent OnPourEnd = null;

    private bool isPouring = false;
    void Update()
    {
        //Debug.DrawRay(transform.position, transform.up);
        if (CalculatePourAngle() && CheckCork())
        {
            if(OnlyPourWhenHeld && bottle != null && !bottle.isSelected)
            {
                return;
            }
            if (!particles.isPlaying)
            {
                GetComponent<ParticleSystem>().Play(true);
                var e = particles.emission;
                e.enabled = true;
                if(OnPour != null && !isPouring)
                {
                    OnPour.Invoke();
                }
                isPouring = true;
            }
        }
        else if (particles.isPlaying)
        {
            GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            if(OnPourEnd != null && isPouring)
            {
                OnPourEnd.Invoke();
            }
            isPouring = false;
        }
    }

    private bool CalculatePourAngle()
    {
        return Vector3.Dot(transform.up, Vector3.down) > 0;

    }
    private bool CheckCork()
    {
        if (!corkSocket)
        {
            return true;
        }
        if (corkSocket && corkSocket.selectTarget)
        {
            return false;
        }
        else if (corkSocket && !corkSocket.selectTarget)
        {
            return true;
        }
        return true;
    }
}
