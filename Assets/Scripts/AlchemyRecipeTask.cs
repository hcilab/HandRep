using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using HighlightPlus;
using Newtonsoft.Json;
using UnityEngine.Events;

public class AlchemyRecipeTask : MonoBehaviour
{
    [SerializeField] private TextAsset ResourceFile;
    [SerializeField] private TMP_Text timerText;

    private Dictionary<string, Ingredient> ingredientList = new Dictionary<string, Ingredient>();
    private MasterRecipes mr;
    private bool hasActiveRecipe = false;

    [SerializeField] private TMP_Text text;
    private Settings settings => FindObjectOfType<Settings>();
    [SerializeField] AudioSource asource;
    [SerializeField] AudioClip successClip;
    [SerializeField] AudioClip failClip;
    [SerializeField] private HighlightEffect hle;

    public UnityEvent<Ingredient[]> OnTaskStart = null;
    public UnityEvent<Ingredient[], Ingredient[], bool, float> OnTaskSubmit = null;

    private float secondsCount;
    private int minuteCount;
    private float taskStartTime;

    public UnityEvent<int> OnTaskStartNum = null;
    public UnityEvent<int, bool> OnTaskEndNum = null;
    private void Start()
    {
        if(!ResourceFile || ResourceFile.text == null)
            ResourceFile = Resources.Load<TextAsset>("potion_recipes.json");
        mr = JsonConvert.DeserializeObject<MasterRecipes>(ResourceFile.text);
        hle.highlighted = true;
    }
    private void Awake()
    {
        text.text = "";
        timerText.text = "";
    }

    public void UpdateTimer()
    {
        //set timer UI
        if (GetTaskActive())
        {
            secondsCount += Time.deltaTime;
            timerText.text = minuteCount + "m:" + (int)secondsCount + "s";
            if (secondsCount >= 60)
            {
                minuteCount++;
                secondsCount = 0;
            }
        }
    }

    public float GetTimerDurationMS()
    {
        return (this.secondsCount + this.minuteCount * 60f) * 1000;
    }

    public bool GetTaskActive()
    {
        return hasActiveRecipe;
    }

    public Dictionary<string, Ingredient> GetTaskIngredients()
    {
        if (GetTaskActive())
        {
            return this.ingredientList;
        }
        return null;
    }

    public void RestartTask()
    {
        hle.highlighted = true;
        text.text = "";
        hasActiveRecipe = false;
        secondsCount = 0;
        minuteCount = 0;
        timerText.text = "";
    }

    public void ActivateTask()
    {
        hle.targetFX = false;
        hle.highlighted = false;
        if (settings.CurrentTier <= settings.MaxTier && !hasActiveRecipe) {
            LoadRecipe(settings.CurrentTier);
            this.ShowIngredients(this.ingredientList);
            secondsCount = 0;
            minuteCount = 0;
            if (OnTaskStart != null)
            {
                List<Ingredient> ing = new List<Ingredient>();
                foreach(Ingredient i in this.ingredientList.Values)
                {
                    ing.Add(i);
                }
                OnTaskStart.Invoke(ing.ToArray());
            }
            if(OnTaskStartNum != null)
            {
                OnTaskStartNum.Invoke(settings.CurrentTier);
            }
        }
    }

    public void SubmitPotion(Ingredient[] ingredients)
    {
        bool match = CompareWithPotion(ingredients);
        if(OnTaskSubmit != null)
        {
            List<Ingredient> ing = new List<Ingredient>();
            foreach (Ingredient i in this.ingredientList.Values)
            {
                ing.Add(i);
            }
            OnTaskSubmit.Invoke(ing.ToArray(), ingredients, match, GetTimerDurationMS());
        }
        if(match)
        {
            Debug.Log("POTION BOTTLE SUCCESS");
            //succeed
            if (!asource.isPlaying)
                asource.PlayOneShot(successClip);
        }
        else
        {
            Debug.Log("POTION BOTTLE FAIL");
            if (!asource.isPlaying)
                asource.PlayOneShot(failClip);
            //fail
        }
        if (OnTaskEndNum != null)
        {
            OnTaskEndNum.Invoke(settings.CurrentTier, match);
        }
        settings.CurrentTier++;
        if (settings.CurrentTier > settings.MaxTier)
        {
            settings.FinishTierTask();
            text.text = "";
            if (settings.orderIndex >= settings.order.Length)
            { // done all!
                text.text = "";
                return;
            }
        }
        hle.highlighted = true;
        text.text = "";
        hasActiveRecipe = false;
        secondsCount = 0;
        minuteCount = 0;
        timerText.text = "";

    }

    private void ShowIngredients(Dictionary<string, Ingredient> ingredients)
    {
        text.text = GetList(ingredients);
    }

    private string GetList(Dictionary<string, Ingredient> ingredients)
    {
        string recipe = "";
        foreach (Ingredient i in ingredients.Values)
        {
            recipe += "• " + i.ToTaskString() + "\n";
        }
        return recipe;
    }

    private void LoadRecipe(int tier, int recipe = -1)
    {
        // if -1 recipe then do random
        ingredientList.Clear();
        List<List<Requirements>> tierObj = null;
        tierObj = tier switch
        {
            1 => mr.tier1,
            2 => mr.tier2,
            3 => mr.tier3,
            //4 => mr.tier4,
            //5 => mr.tier5,
            _ => mr.tier1,
        };
        if (recipe == -1 || recipe < 0 || recipe >= tierObj.Count) {
            recipe = Random.Range(0, tierObj.Count);
        }
        List<Requirements> selectedRecipe = tierObj[recipe];
        foreach(Requirements r in selectedRecipe)
        {
            //(string key, string name, int count, float quantity, bool isSolid, bool isTemp
            Ingredient i = new Ingredient();
            i.isFromRecipe = true;
            i.key = r.key;
            i.name = r.clean_name;
            i.isTemp = r.key.Equals("temperature");
            if(r.quantifier.Equals("quantity"))
            {
                i.constraints = Ingredient.Constraints.Quantity;
                i.maxQuantity = r.max;
                i.minQuantity = r.min;
            }
            else if(r.quantifier.Equals("count"))
            {
                i.constraints = Ingredient.Constraints.Count;
                i.maxCount = (int)r.max;
                i.minCount = (int)r.min;
            }
            else
            {
                i.constraints = Ingredient.Constraints.None;
            }
            if(r.tag.Equals("ml"))
            {
                i.isSolid = false;
            }
            else if(r.tag.Equals("g"))
            {
                i.isSolid = true;
            }
            else
            {
                // tag is °C for istemp
                i.isSolid = false;
            }
            ingredientList.Add(i.key, i);
        }
        if(ingredientList.Values.Count > 0)
            hasActiveRecipe = true;
        
    }

    private bool CompareWithPotion(Ingredient[] ingredients)
    {
        if(ingredients == null)
        {
            return false; ////////////////////////////////////////////////
        }
        Dictionary<string, Ingredient> bottleList = new Dictionary<string, Ingredient>();
        foreach(Ingredient i in ingredients)
        {
            bottleList.Add(i.key, i);
        }
        bool matches = bottleList.Count == this.ingredientList.Count;
        //Debug.Log(bottleList.Count + "     ___  " + this.ingredientList.Count);
        if(matches)
        {
            foreach(Ingredient i in this.ingredientList.Values)
            {
                
                if (bottleList.TryGetValue(i.key, out Ingredient i2))
                {
                    //Debug.Log(i.key + "    _      " + i.maxQuantity + "|"+i2.quantity+  " ___ " + i2.maxCount+"|"+ i.count);
                    matches = matches && i.CompareRanges(i2);
                }
                else
                {
                    matches = false;
                }
                if(!matches)
                {
                    break;
                }
            }
        }

        return matches;
    }

    private void Update()
    {
        UpdateTimer();
    }

    [System.Serializable]
    public class MasterRecipes
    {
        public List<List<Requirements>> tier1 { get; set; }
        public List<List<Requirements>> tier2 { get; set; }
        public List<List<Requirements>> tier3 { get; set; }
        //public List<List<Requirements>> tier4 { get; set; }
        //public List<List<Requirements>> tier5 { get; set; }
    }

    [System.Serializable]
    public class Requirements
    {
        public float min { get; set; } = 0f;
        public float max { get; set; } = 0f;
        public string clean_name { get; set; }
        public string quantifier { get; set; } = "";
        public string tag { get; set; } = "";
        public string key { get; set; }
    }

}
