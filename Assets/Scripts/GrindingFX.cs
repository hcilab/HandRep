using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrindingFX : MonoBehaviour
{

    [Header("Shake Info")]
    private Vector3 _startPos;
    //private float _timer;
    private Vector3 _randomPos;

    [Header("Shake Settings")]
    //[Range(0f, 2f)]
    //public float _time = 0.2f;
    [Range(0f, 0.02f)]
    public float _distance = 0.005f;
    [Range(0f, 0.1f)]
    public float _delayBetweenShakes = 0f;

    [SerializeField] ParticleSystem particles;
    [SerializeField] Transform attach;
    private XRPropertySocket socket => GetComponent<XRPropertySocket>();


    private void Awake()
    {
        _startPos = attach.position;
        if (particles.isPlaying)
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private void OnValidate()
    {
        //if (_delayBetweenShakes > _time)
        //    _delayBetweenShakes = _time;
    }

    public void Begin()
    {
        End();
        StartCoroutine(Shake());
        if(!particles.isPlaying)
        {
            particles.Play(true);
            var e = particles.emission;
            e.enabled = true;
        }
    }

    public void End()
    {
        StopAllCoroutines();
        if (particles.isPlaying)
        {
            particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private IEnumerator Shake()
    {
        //_timer = 0f;

        while (true)//_timer < _time)
        {
            //_timer += Time.deltaTime;

            _randomPos = _startPos + (Random.insideUnitSphere * _distance);

            attach.position = _randomPos;

            if (_delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(_delayBetweenShakes);
            }
            else
            {
                yield return null;
            }
        }

        attach.position = _startPos;
    }
}
