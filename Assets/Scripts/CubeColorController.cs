using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

//[RequireComponent(typeof(CubeLogger))]
public class CubeColorController : MonoBehaviour
{
    [SerializeField] Material blankMat;
    [SerializeField] Material redMat;
    [SerializeField] Material purpleMat;
    [SerializeField] Material blueMat;
    [SerializeField] Material greenMat;
    [SerializeField] Material yellowMat;
    [SerializeField] Material orangeMat;

    [SerializeField] GameObject[] gemstones = new GameObject[6];

    private int index = -1;
    private GemColour currentColour = GemColour.Blank;

    [SerializeField] private bool showDebugLabels = false;
    [SerializeField] private TMP_Text[] labels = new TMP_Text[6];

    //private CubeLogger clog => GetComponent<CubeLogger>();
    private XRBaseInteractable interactable => GetComponent<XRBaseInteractable>();
    public UnityEvent<CubeChangeArgs> OnActivate = null;
    public enum GemColour
    {
        Blank, Red, Purple, Blue, Green, Yellow, Orange
    }

    private GemColour[] validColours = { GemColour.Red, GemColour.Purple, GemColour.Blue,
        GemColour.Green, GemColour.Yellow, GemColour.Orange};

    private int currentColourIndex = -1;
    public class CubeChangeArgs
    {
        public GemColour oldColour;
        public int oldIndex;
        public GemColour newColour;
        public int newIndex;

        public CubeChangeArgs(GemColour old, int oldi, GemColour newC, int newi)
        {
            this.oldColour = old;
            oldIndex = oldi;
            newColour = newC;
            newIndex = newi;
        }
    }

    private void Awake()
    {
        GetComponent<ObjectProperties>().AddOrSet("ingredient_type", currentColour.ToString().ToLower() + "_puzzle_cube");
        GetComponent<ObjectProperties>().AddOrSet("current_colour", currentColour.ToString());
        GetComponent<ObjectProperties>().AddOrSet("current_side", index.ToString());
        GetComponent<ObjectProperties>().AddOrSet("clean_name", currentColour.ToString() + " Puzzle Cube");

        if (showDebugLabels && labels.Length != 0)
        {
            foreach(TMP_Text t in labels)
            {
                t.text = "";
            }
        }
    }

    public void RandomizeColour()
    {
        GemColour oldColour = GetColour();
        int oldIndex = GetIndex();

        if (oldIndex >= 0 && showDebugLabels && labels.Length > 0)
        {
            labels[oldIndex].text = "";
        }

        GemColour newColour = currentColour;
        int newCIndex = currentColourIndex;
        if (currentColourIndex == -1)
        {
            newCIndex = Random.Range(0, validColours.Length);
            newColour = validColours[newCIndex];
        }
        else
        {
            newCIndex = System.Array.IndexOf(validColours, currentColour);
            newCIndex += 1;
            if(newCIndex>= validColours.Length)
            {
                newCIndex = 0;
            }
            newColour = validColours[newCIndex];
        }
        Debug.Log(oldColour + "  " + newCIndex + "   " + newColour);


        int newIndex = Random.Range(0, gemstones.Length);
        while (newIndex == index)
        {
            newIndex = Random.Range(0, gemstones.Length);
        }
        index = newIndex;

        foreach(GameObject go in gemstones)
        {
            go.GetComponent<MeshRenderer>().material = blankMat;
        }

        //GemColour newColour = validColours[Random.Range(0, validColours.Length)];
//        GemColour newColour = validColours[newCIndex];
        /*while (newColour == currentColour)
        {
            newColour = validColours[Random.Range(0, validColours.Length)];
        }*/

        currentColour = newColour;
        currentColourIndex = newCIndex;

        gemstones[index].GetComponent<MeshRenderer>().material = GetMaterial(currentColour);
        UpdateProperties(false);
        if(showDebugLabels && labels.Length > 0)
        {
            labels[index].text = currentColour.ToString();
        }

        Settings settings = FindObjectOfType<Settings>();
        if(settings.gameObject.TryGetComponent<TaskSummaryLog>(out TaskSummaryLog tsl))
        {
            tsl.IncrementCubeChange();
        }

        if(OnActivate != null)
        {
            CubeChangeArgs args = new CubeChangeArgs(oldColour, oldIndex, currentColour, index);
            OnActivate.Invoke(args);
        }
    }

    public void UpdateProperties(bool log = true)
    {
        GetComponent<ObjectProperties>().AddOrSet("ingredient_type", currentColour.ToString().ToLower() + "_puzzle_cube");
        GetComponent<ObjectProperties>().AddOrSet("current_colour", currentColour.ToString());
        GetComponent<ObjectProperties>().AddOrSet("current_side", index.ToString());
        GetComponent<ObjectProperties>().AddOrSet("clean_name", currentColour.ToString() + " Puzzle Cube");
        /*if (clog && log)
        {
            clog.LogCubeEvent(interactable, currentColour, index, "post");
        }*/
    }

    private Material GetMaterial(GemColour colour)
    {
        Material newMat = blankMat;
        switch (colour)
        {
            case GemColour.Red:
                newMat = redMat;
                break;
            case GemColour.Purple:
                newMat = purpleMat;
                break;
            case GemColour.Yellow:
                newMat = yellowMat;
                break;
            case GemColour.Orange:
                newMat = orangeMat;
                break;
            case GemColour.Blue:
                newMat = blueMat;
                break;
            case GemColour.Green:
                newMat = greenMat;
                break;
            default:
                break;
        }
        return newMat;
    }

    public GemColour GetColour()
    {
        return currentColour;
    }

    public int GetIndex()
    {
        return index;
    }

    public override string ToString()
    {
        return currentColour.ToString() + " - " + index;
    }
}
