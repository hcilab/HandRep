using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;


public class GrindingBowlHelper : MonoBehaviour
{
    public UnityEvent OnGrindStart = null;
    public UnityEvent OnGrindEnd = null;
    public UnityEvent OnRotationComplete = null;
    public UnityEvent OnGrindComplete = null;
    [SerializeField] HingeJoint mortarHinge;
    [SerializeField] XRPropertySocket socket;
    [SerializeField] XRGrabInteractable pestle;
    [SerializeField] float maxDistance = 0.085f;
    [SerializeField] Transform distanceIndicator;
    [SerializeField] float[] targetsForTurn = { 1f, 180f, 270f, 350f };
    private int indexTarget = -1;
    private int fullRotationsCount = 0;
    [SerializeField] int rotationsNeeded = 6;

    private Vector3 pestleInitialPos;
    private Quaternion pestleInitialRot;

    [SerializeField] private GrindingFX fx;
    private bool isGrinding = false;

    [SerializeField] private AudioSource pestleSoundSource;
    [SerializeField] private AudioClip[] pestleSounds;

    private float lastVal = 0f;
    //private GrinderLogger glog => GetComponent<GrinderLogger>();
    private void Awake()
    {
        if(!mortarHinge || !socket || !pestle)
        {
            Destroy(this);
        }
        socket.socketActive = true;
        socket.allowSelect = true;
        DisableGrinding();
    }

    private void Start()
    {
        pestleInitialPos = pestle.transform.position;
        pestleInitialRot = pestle.transform.rotation;
    }

    public void ResetBowl()
    {
        indexTarget = 0;
        fullRotationsCount = 0;
        if(socket.selectTarget != null)
        {
            Destroy(socket.selectTarget.gameObject);
        }
        DisableGrinding();
        LetGo(null);
    }

    private void Update()
    {
        if (Vector3.Distance(distanceIndicator.transform.position,
            socket.attachTransform.position) > maxDistance && pestle.isSelected)
        {
            pestle.interactionManager.CancelInteractorSelection(pestle.selectingInteractor);

        }

        if (!socket.selectTarget)
        {
            if(isGrinding)
            {
                fx.End();
                isGrinding = false;
                if (OnGrindEnd != null)
                {
                    OnGrindEnd.Invoke();
                    //glog.LogGrindEvent(null, "stop");
                }
            }
            // no item, don't care about grinding
            return;
        }

        if (pestle.isSelected)
        {
            if(!isGrinding)
            {
                isGrinding = true;
                fx.Begin();
                if (OnGrindEnd != null)
                {
                    OnGrindStart.Invoke();
                    //glog.LogGrindEvent(socket.selectTarget, "start");
                }
            }
            float val = mortarHinge.angle < 0 ? mortarHinge.angle + 360f : mortarHinge.angle;
            //moving
            if (TryGetComponent<IDial>(out IDial idial))
            {
                if (lastVal != val)
                {
                    foreach (IDial dial in GetComponents<IDial>())
                    {
                        dial.DialUpdate(val);
                    }
                }
                lastVal = val;
            }

            // I do not currently handle the case if they rotated counterclockwise. Only clockwise
            int rounded = Mathf.RoundToInt(Mathf.Abs(val)) % 15;
            if(isGrinding && rounded <= 2 && pestleSoundSource && !pestleSoundSource.isPlaying)
            {
                pestleSoundSource.PlayOneShot(pestleSounds[Random.Range(0, pestleSounds.Length)]);
            }

            if (val > targetsForTurn[0] && indexTarget == -1)
            {
                indexTarget = 0;
            }

            if (indexTarget + 1 >= targetsForTurn.Length 
                || (val > targetsForTurn[indexTarget]
                    && (indexTarget + 1 >= targetsForTurn.Length
                    || val < targetsForTurn[indexTarget + 1])))
            {
                indexTarget += 1;
                if (indexTarget >= targetsForTurn.Length)
                {
                    indexTarget = -1;
                    // we did a rotation;
                    /*if(glog)
                    {
                        glog.LogGrindEvent(socket.selectTarget, "fullrotation");
                    }*/
                    fullRotationsCount += 1;
                    if(OnRotationComplete != null)
                    {
                        OnRotationComplete.Invoke();
                    }
                    if (fullRotationsCount >= rotationsNeeded)
                    {
                        if(OnGrindComplete != null)
                        {
                            OnGrindComplete.Invoke();
                        }
                        fullRotationsCount = 0;

                        socket.selectTarget.GetComponent<GrindableIngredientHelper>().Grind();
                        DisableGrinding();
                        LetGo(null);

                    }
                }
            }
        }
        else
        {
            if (isGrinding)
            {
                fx.End();
                isGrinding = false;
                if (OnGrindEnd != null)
                {
                    OnGrindEnd.Invoke();
                    //glog.LogGrindEvent(socket.selectTarget, "stop");
                }
            }
        }

    }

    private void OnEnable()
    {
        pestle.selectExited.AddListener(LetGo);
        socket.selectEntered.AddListener(EnableGrinding);
        socket.selectExited.AddListener(ChangeProperties);
    }

    private void OnDisable()
    {
        pestle.selectExited.RemoveListener(LetGo);
        socket.selectEntered.RemoveListener(EnableGrinding);
        socket.selectExited.RemoveListener(ChangeProperties);
    }

    private void LetGo(SelectExitEventArgs args)
    {
        pestle.transform.position = pestleInitialPos;
        pestle.transform.rotation = pestleInitialRot;
    }

    private void EnableGrinding(SelectEnterEventArgs args)
    {
        pestle.enabled = true;
        pestle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
        args.interactable.interactionLayerMask = LayerMask.NameToLayer("IgnoreAll");

        foreach (Collider col in args.interactable.colliders)
        {
            Physics.IgnoreCollision(pestle.colliders[0], col);
        }
    }

    private void ChangeProperties(SelectExitEventArgs args)
    {
        foreach (Collider col in args.interactable.colliders)
        {
            Physics.IgnoreCollision(pestle.colliders[0], col);
        }
        if (args.interactable.GetComponent<ObjectProperties>().CompareProperty("grindable", "false"))
        {
            /*args.interactable.GetComponent<ObjectProperties>().AddOrSet("mortar_insert", "false");
            args.interactable.GetComponent<ObjectProperties>().AddOrSet("isDust", "true");
            args.interactable.GetComponent<ObjectProperties>().AddOrSet("enchantable", "false");
            args.interactable.GetComponent<ObjectProperties>().AddOrSet("clean_name", "Ground " +
                args.interactable.GetComponent<ObjectProperties>().properties.Find((p) =>
                   p.key == "clean_name").value);
            args.interactable.GetComponent<ObjectProperties>().AddOrSet("ingredient_type", "grnd_" +
                args.interactable.GetComponent<ObjectProperties>().properties.Find((p) =>
                   p.key == "ingredient_type").value);
            */
        }
    }

    private void DisableGrinding()
    {
        pestle.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        pestle.enabled = false;

    }

}
