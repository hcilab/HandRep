using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.IO;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public enum HandModelSet
{
    Hand,
    Index,
    Oculus,
    Wand,
    Sphere,
    None
}
public class Settings : MonoBehaviour
{
    public delegate void HandModelSetUpdate(HandModelSet set);
    public static event HandModelSetUpdate OnHandUpdate;

    public delegate void ToggleRayInteractorsUpdate(bool enabled);
    public static event ToggleRayInteractorsUpdate OnRayToggleUpdate;

    private bool areRaysEnabled = false;

    [SerializeField] private HandModelSet CurrentHandModelSet = HandModelSet.None;

    [SerializeField] private GameObject RightHandModel;
    [SerializeField] private GameObject LeftHandModel;

    [SerializeField] private GameObject RightIndexModel;
    [SerializeField] private GameObject LeftIndexModel;

    [SerializeField] private GameObject RightOculusModel;
    [SerializeField] private GameObject LeftOculusModel;

    [SerializeField] private GameObject RightWandModel;
    [SerializeField] private GameObject LeftWandModel;

    [SerializeField] private GameObject RightSphereModel;
    [SerializeField] private GameObject LeftSphereModel;

    //[SerializeField] private float seatedMinHeight = 1.56144f;
    //[SerializeField] private float standingMinHeight = 1.36144f;
    [SerializeField] private float seatedOffset = 0.2f; //1.36144f;
    //[SerializeField] private float standingOffset = 1.0f;
    [SerializeField] private Transform CameraOffset;

    //public bool isLPoseActive = false;
    //public bool isRPoseActive = false;

    public bool isSeated = false;

    public GameObject currentLeftModel { get; private set; }
    public GameObject currentRightModel { get; private set; }

    public HandModelSet GetCurrentHandModelSet() => CurrentHandModelSet;

    public Transform startPoint;

    private string participantID { get; set; }  = System.DateTime.Now.ToString("yyyyMMddHHmmss");

    // A = Sphere
    // B = Index
    // C = Hand
    // ABC, ACB, BAC, BCA, CAB, CBA
    private HandModelSet[][] orders = new HandModelSet[][] {
        new HandModelSet[] {HandModelSet.Sphere, HandModelSet.Index, HandModelSet.Hand},
        new HandModelSet[] {HandModelSet.Sphere, HandModelSet.Hand, HandModelSet.Index},
        new HandModelSet[] {HandModelSet.Index, HandModelSet.Sphere, HandModelSet.Hand},
        new HandModelSet[] {HandModelSet.Index, HandModelSet.Hand, HandModelSet.Sphere},
        new HandModelSet[] {HandModelSet.Hand, HandModelSet.Sphere, HandModelSet.Index},
        new HandModelSet[] {HandModelSet.Hand, HandModelSet.Index, HandModelSet.Sphere},
    };
    public HandModelSet[] order = null;
    public int orderIndex = 0;

    [SerializeField] public int CurrentTier { get; set; } = 0; ///////// set to 0!!! then 1 for real game
    [SerializeField] public int MaxTier = 5;

    public CmdLineManager cmdLine => FindObjectOfType<CmdLineManager>();
    public UnityEvent OnResetPress = null;
    private static string GetArg(string name)
    {
        string[] args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i<args.Length; i++)
        {
            if(args[i] == name && args.Length > i + 1)
            {
                return args[i + 1];
            }
        }
        return null;
    }

    public void ResetTask()
    {
        if(OnResetPress != null)
        {
            OnResetPress.Invoke();
        }
    }

    public bool FinishT_Task()
    {
        orderIndex++;
        if (orderIndex >= order.Length)
        {
            Debug.Log("Done all hand reps");
            // done all 3;
            // spawn button to go to next scene
            return true;
        }
        else
        {
            SetHandModelSet(order[orderIndex]);
            return false;
        }
    }

    public void FinishTierTask()
    {
        if (CurrentTier >= 1) // never call this when in training tiers
        {
            orderIndex++;
            if(orderIndex >= order.Length)
            {
                Debug.Log("Done all hand reps");
                // done all 3;
                // do final questionnaire then quit
            }
            else
            {
                // do questionnaire then next
                // in all questions, disable DirectHands
                SetHandModelSet(order[orderIndex]);
                CurrentTier = 1;
            }
        }
    }

    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public string GetPID()
    {
        return cmdLine ? cmdLine.participantID : participantID;
    }
    public void StartGame()
    {
        CurrentTier = 1;
        orderIndex = 0;
    }

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainGame"))
        {
            CurrentTier = 1;
        }
        string orderArg = GetArg("-order");
        if (orderArg != null && !orderArg.Equals(null))
        {
            order = orders[int.Parse(orderArg)];
        }
        else
        {
            // error we need to back out but for now i hardcode number 0
#if UNITY_EDITOR
            order = orders[0];
#elif !UNITY_EDITOR
            Application.Quit();
#endif
        }
        /*
        string seatArg = GetArg("-seated");
        if (seatArg != null && !seatArg.Equals(null) && seatArg.Equals("true"))
        {
            isSeated = true;
        }
        else if (seatArg != null && !seatArg.Equals(null) && seatArg.Equals("false"))
        {
            isSeated = false;
        }
        
        string idArg = GetArg("-pid");
        if (idArg != null && !idArg.Equals(null))
        {
            participantID = idArg;
        }*/

        participantID = cmdLine ? cmdLine.participantID : participantID + "_" + "Testing";
        isSeated = cmdLine ? cmdLine.isSeated : true;


        //FindObjectOfType<XRRig>().trackingOriginMode = UnityEngine.XR.TrackingOriginModeFlags.Device;
        if (isSeated)
        {
            //FindObjectOfType<CharacterControllerDriver>().minHeight = seatedMinHeight;

            //FindObjectOfType<XRRig>().transform.GetChild(0).transform.localScale = new Vector3( 1f,seatedOffset, 1f);
            CameraOffset.localPosition = new Vector3(0f, seatedOffset, 0f);
        }
        else
        {
            //FindObjectOfType<CharacterControllerDriver>().minHeight = standingMinHeight;
            //FindObjectOfType<XRRig>().cameraYOffset = standingOffset;
        }

        if (startPoint)
        {
            FindObjectOfType<XRRig>().gameObject.transform.position = startPoint.transform.position;
        }
        string dataDirectory = Application.persistentDataPath + "/DATA";
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }
        dataDirectory += "/" + participantID;
        if (!Directory.Exists(dataDirectory))
        {
            Directory.CreateDirectory(dataDirectory);
        }
        if (!Directory.Exists(dataDirectory + "/Game"))
        {
            Directory.CreateDirectory(dataDirectory + "/Game");
        }
        if (!Directory.Exists(dataDirectory + "/Training"))
        {
            Directory.CreateDirectory(dataDirectory + "/Training");
        }

        if (SceneManager.GetActiveScene().name.Equals("MainGame"))
        {
            StartGame();
        }
        DisableAllHands();
    }

    private void Start()
    {
        DisableAllHands();
        CurrentHandModelSet = order[orderIndex];
        SetHandModelSet(CurrentHandModelSet);
        Settings.OnRayToggleUpdate(areRaysEnabled);
        DisableAllButCurrentHands();
        if(!CurrentHandModelSet.Equals(HandModelSet.Sphere) && (RightSphereModel.activeInHierarchy
            || LeftSphereModel.activeInHierarchy))
        {
            DisableModel(RightSphereModel);
            DisableModel(LeftSphereModel);
        }
    }

    private void Update()
    {
        if (isSeated)
        {
            CameraOffset.localPosition = new Vector3(0f, seatedOffset, 0f);
        }
        else
        {
        }
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            // Tell participant to drop anything/everything
            ResetTask();
        }
        if(Keyboard.current.pKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (!CurrentHandModelSet.Equals(HandModelSet.Sphere) && (RightSphereModel.activeInHierarchy
            || LeftSphereModel.activeInHierarchy))
        {
            DisableModel(RightSphereModel);
            DisableModel(LeftSphereModel);
        }
    }

    public void SetHandModelSet(HandModelSet set)
    {
        Settings.OnHandUpdate(set);
    }

    public void EnableRays()
    {
        OnRayToggleUpdate(true);
    }

    public void DisableRays()
    {
        OnRayToggleUpdate(false);
    }

    private void OnEnable()
    {
        OnHandUpdate += SetCurrentHandModelSet;
        OnRayToggleUpdate += ToggleRays;
    }

    private void OnDisable()
    {
        OnHandUpdate -= SetCurrentHandModelSet;
        OnRayToggleUpdate -= ToggleRays;
    }

    private void ToggleRays(bool enabled)
    {
        areRaysEnabled = enabled;
    }

    public void ToggleUIRays()
    {
        OnRayToggleUpdate(!areRaysEnabled);
    }

    private void SetCurrentHandModelSet(HandModelSet set)
    {
        DisableAllHands();
        CurrentHandModelSet = set;
        SetModels(CurrentHandModelSet);
    }

    private void SetModels(HandModelSet set)
    {
        switch (set)
        {
            case HandModelSet.Index:
                currentRightModel = RightIndexModel;
                currentLeftModel = LeftIndexModel;
                break;
            case HandModelSet.Oculus:
                currentRightModel = RightOculusModel;
                currentLeftModel = LeftOculusModel;
                break;
            case HandModelSet.Wand:
                currentRightModel = RightWandModel;
                currentLeftModel = LeftWandModel;
                break;
            case HandModelSet.Hand:
                currentRightModel = RightHandModel;
                currentLeftModel = LeftHandModel;
                break;
            case HandModelSet.Sphere:
                currentRightModel = RightSphereModel;
                currentLeftModel = LeftSphereModel;
                break;
            default:
                currentRightModel = RightSphereModel;
                currentLeftModel = LeftSphereModel;
                break;
        }
        if (!currentRightModel)
        {
            currentRightModel = RightSphereModel;
        }
        if (!currentLeftModel)
        {
            currentLeftModel = LeftSphereModel;
        }
        EnableModel(currentLeftModel);
        EnableModel(currentRightModel);
        DisableAllButCurrentHands();
    }

    public GameObject GetBaseModelForHand(XRDirectInteractor interactor)
    {
        if(!currentLeftModel || !currentRightModel)
        {
            DisableAllHands();
            SetModels(CurrentHandModelSet);
        }
        SetCurrentModelVariables();
        DisableAllButCurrentHands();
        if (interactor.CompareTag("LeftHand"))
        {
            return currentLeftModel;
        }
        else if(interactor.CompareTag("RightHand"))
        {
            return currentRightModel;
        }
        return null;
    }

    public void DisableModelForHand(XRDirectInteractor interactor)
    {
        SetCurrentModelVariables();
        DisableAllButCurrentHands();
        if (interactor.CompareTag("LeftHand"))
        {
            DisableModel(currentLeftModel);
            interactor.GetComponent<ActionBasedController>().model = null;
        }
        else if (interactor.CompareTag("RightHand"))
        {
            DisableModel(currentRightModel);
            interactor.GetComponent<ActionBasedController>().model = null;
        }
    }

    public void EnableModelForHand(XRDirectInteractor interactor)
    {
        SetCurrentModelVariables();
        DisableAllButCurrentHands();

        if (interactor.CompareTag("LeftHand"))
        {
            /*if(isLPoseActive && (CurrentHandModelSet.Equals(HandModelSet.Hand) || 
                CurrentHandModelSet.Equals(HandModelSet.Sphere)))
            {
                DisableModelForHand(interactor);
                return;
            }*/
            EnableModel(currentLeftModel);
            interactor.GetComponent<ActionBasedController>().model = currentLeftModel.transform;
        }
        else if (interactor.CompareTag("RightHand"))
        {
            /*if (isRPoseActive && (CurrentHandModelSet.Equals(HandModelSet.Hand) ||
                CurrentHandModelSet.Equals(HandModelSet.Sphere)))
            {
                DisableModelForHand(interactor);
                return;
            }*/
            EnableModel(currentRightModel);
            interactor.GetComponent<ActionBasedController>().model = currentRightModel.transform;
        }
    }

    private void DisableAllButCurrentHands()
    {
        if (!RightHandModel.Equals(currentRightModel)) DisableModel(RightHandModel);
        if (!LeftHandModel.Equals(currentLeftModel)) DisableModel(LeftHandModel);
        if (!RightIndexModel.Equals(currentRightModel)) DisableModel(RightIndexModel);
        if (!LeftIndexModel.Equals(currentLeftModel)) DisableModel(LeftIndexModel);
        if (!RightSphereModel.Equals(currentRightModel)) DisableModel(RightSphereModel);
        if (!LeftSphereModel.Equals(currentLeftModel)) DisableModel(LeftSphereModel);
    }

    private void SetCurrentModelVariables()
    {
        switch (CurrentHandModelSet)
        {
            case HandModelSet.Index:
                currentRightModel = RightIndexModel;
                currentLeftModel = LeftIndexModel;
                break;
            case HandModelSet.Oculus:
                currentRightModel = RightOculusModel;
                currentLeftModel = LeftOculusModel;
                break;
            case HandModelSet.Wand:
                currentRightModel = RightWandModel;
                currentLeftModel = LeftWandModel;
                break;
            case HandModelSet.Hand:
                currentRightModel = RightHandModel;
                currentLeftModel = LeftHandModel;
                break;
            case HandModelSet.Sphere:
                currentRightModel = RightSphereModel;
                currentLeftModel = LeftSphereModel;
                break;
            default:
                currentRightModel = RightSphereModel;
                currentLeftModel = LeftSphereModel;
                break;
        }
        if (!currentRightModel)
        {
            currentRightModel = RightSphereModel;
        }
        if (!currentLeftModel)
        {
            currentLeftModel = LeftSphereModel;
        }
    }

    public void DisableAllHands()
    {
        DisableModel(RightHandModel);
        DisableModel(LeftHandModel);
        DisableModel(RightIndexModel);
        DisableModel(LeftIndexModel);
        DisableModel(RightSphereModel);
        DisableModel(LeftSphereModel);
    }

    private void DisableModel(GameObject model)
    {
        foreach(MeshRenderer mr in model.GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
        }
        model.SetActive(false);
    }

    private void EnableModel(GameObject model)
    {
        model.SetActive(true);
        foreach (MeshRenderer mr in model.GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = true;
        }
    }
}
