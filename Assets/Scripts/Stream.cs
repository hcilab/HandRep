using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    private LineRenderer lineRenderer => GetComponent<LineRenderer>();
    private ParticleSystem splash = null;
    private Coroutine pourRoutine = null;

    private Vector3 targetPosition = Vector3.zero;

    private Coroutine particleRoutine = null;

    private void Awake()
    {
        splash = GetComponentInChildren<ParticleSystem>();
        lineRenderer.positionCount = 0;
    }

    public int CheckPoints()
    {
        return lineRenderer.positionCount;
    }

    public void Begin()
    {
        if(particleRoutine == null)
            particleRoutine = StartCoroutine(UpdateParticles());
        if (pourRoutine != null)
            StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(BeginPouring());
    }

    private IEnumerator BeginPouring()
    {
        lineRenderer.positionCount = 2;
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
        while (gameObject.activeSelf)
        {
            Vector3 pos = FindEndPoint();
            if (pos != transform.position)
            {
                targetPosition = pos;
                MoveToPosition(0, transform.position);
                AnimateToPosition(1, targetPosition);
            }
            else
            {
                End();
            }
            yield return null;
        }
        
    }

    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 2.0f);
        if (hit.collider &&
            hit.collider.gameObject.transform.IsChildOf(transform.parent))
        {
            return transform.position;
        }
        Vector3 end = hit.collider ? hit.point : ray.GetPoint(2.0f);
        return end;
    }

    private void MoveToPosition(int index, Vector3 targetPos)
    {
        lineRenderer.SetPosition(index, targetPos);
    }

    public void End()
    {
        if(pourRoutine != null)
            StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }

    private IEnumerator EndPour()
    {
        while (lineRenderer.positionCount > 0)//(!HasReachedPosition(0, targetPosition))
        {
            AnimateToPosition(0, targetPosition);
            AnimateToPosition(1, targetPosition);
            if(HasReachedPosition(0, targetPosition))
            {
                lineRenderer.positionCount = 0;
                splash.gameObject.SetActive(false);
                //Destroy(this);
            }
            yield return null;
        }
    }

    private void AnimateToPosition(int index, Vector3 targetPos)
    {
        Vector3 current = lineRenderer.GetPosition(index);
        Vector3 newPos = Vector3.MoveTowards(current, targetPos, Time.deltaTime * 2f);
        lineRenderer.SetPosition(index, newPos);
    }

    private bool HasReachedPosition(int index, Vector3 targetPos)
    {
        if (lineRenderer.positionCount > 0)
        {
            Vector3 currentPos = lineRenderer.GetPosition(index);
            return currentPos == targetPos;
        }
        return true;
    }

    private IEnumerator UpdateParticles()
    {
        while (gameObject.activeSelf)
        {
            if (lineRenderer.positionCount > 0)
            {
                splash.gameObject.transform.position = lineRenderer.GetPosition(1);
                bool isHitting = HasReachedPosition(1, targetPosition);
                splash.gameObject.SetActive(isHitting);
            }
            else
            {
                splash.gameObject.SetActive(false);
            }
            yield return null;
        }
    } 
}
