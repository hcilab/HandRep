using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CauldronDataLog : BaseDataLog
{
    private enum State
    {
        None,
        Cleared,
        BottleFilled,
        ItemAdded
    }
    public void Start()
    {
        string headers = this.GetMandatoryHeaders();
        headers += ",State";
        headers += ",Ingredient Name"; // name
        headers += ",Ingredient Type"; // key
        headers += ",Ingredient Weight";
        headers += ",Ingredient Count";
        headers += ",Contents"; // sep by |
        WriteHeader(this.filename, headers, this.filename);
    }
    public void LogAddEvent(Ingredient item, Dictionary<string, Ingredient> contents)
    {
        State state = State.ItemAdded;

        string cleanName = item.name;
        string type = item.key;
        string weight = item.quantity.ToString("F2");
        string count = item.count.ToString();

        string line = "";
        line += state.ToString();
        line += "," + cleanName;
        line += "," + type;
        line += "," + weight;
        line += "," + count;
        line += "," + ContentsString(contents);
        Write(this.filename, line, this.filename);
    }

    public void LogFillEvent(Dictionary<string, Ingredient> contents)
    {
        State state = State.BottleFilled;

        string cleanName = "none";
        string type = "none";
        string weight = "none";
        string count = "none";

        string line = "";
        line += state.ToString();
        line += "," + cleanName;
        line += "," + type;
        line += "," + weight;
        line += "," + count;
        line += "," + ContentsString(contents);
        Write(this.filename, line, this.filename);
    }

    public void LogClearEvent()
    {
        State state = State.Cleared;

        string cleanName = "none";
        string type = "none";
        string weight = "none";
        string count = "none";

        string line = "";
        line += state.ToString();
        line += "," + cleanName;
        line += "," + type;
        line += "," + weight;
        line += "," + count;
        line += "," + "none";
        Write(this.filename, line, this.filename);
    }

    private string ContentsString(Dictionary<string, Ingredient> ingredients)
    {
        string s = "";
        foreach(Ingredient i in ingredients.Values)
        {
            s += i.ToString();
            s += "|";
        }
        s = s.Remove(s.Length - 1, 1);
        return s;
    }
}
