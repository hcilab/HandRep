using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string key;
    public string name;
    public int count;
    public float quantity;
    public bool isSolid;
    public bool isTemp;
    public bool isFromRecipe = false;

    public enum Constraints
    {
        None, 
        Count,
        Quantity
    }
    public Constraints constraints = Constraints.None;

    public float minQuantity = 0.0f;
    public float maxQuantity = 10.0f;
    public int minCount = 1;
    public int maxCount = 10;

    public Ingredient()
    {

    }

    public Ingredient(string key, string name, int count, float quantity, bool isSolid)
    {
        this.key = key;
        this.name = name;
        this.count = count;
        this.quantity = quantity;
        this.isSolid = isSolid;
        this.isTemp = false;
    }

    public Ingredient(string key, string name, int count, float quantity, bool isSolid, bool isTemp)
    {
        this.key = key;
        this.name = name;
        this.count = count;
        this.quantity = quantity;
        this.isSolid = isSolid;
        this.isTemp = true;
    }


    public void AddAmounts(Ingredient i)
    {
        this.count += 1;
        this.quantity += i.quantity;
    }

    public override string ToString()
    {
        string s = "";
        string q = isSolid ? this.quantity.ToString("F2") + "g" : this.quantity.ToString("F2") + "ml";
        if (isSolid)
            s = string.Format("{0}x ({1}) - {2}", this.count, q, this.name);
        else
            s = string.Format("{0} - {1}", q, this.name);
        if(isTemp)
        {
            s = string.Format("{0} - {1}", this.quantity + "°C", this.name);
        }
        return s;
    }

    public string ToTaskString()
    {
        string s = "";
        switch (constraints)
        {
            case Constraints.None:
                return this.ToString();
            case Constraints.Count:
                if (minCount == maxCount)
                    s = string.Format("{0}x {1}", this.maxCount, this.name);
                else
                    s = string.Format("{0}-{1}x {2}", this.minCount, this.maxCount, this.name);
                break;
            case Constraints.Quantity:
                string tag = "";
                if (isSolid)
                    tag = "g";
                else if (!isTemp)
                    tag = "ml";
                else
                    tag = "°C";
                if (minCount == maxCount)
                    s = string.Format("{0}{1} {2}", this.maxQuantity, tag, this.name);
                else
                    s = string.Format("{0}-{1}{2} {3}", this.minQuantity, this.maxQuantity, tag, this.name);
                break;
            default:
                return this.ToString();
        }
        return s;
    }

    public bool CompareSelfRanges()
    {
        if(constraints == Constraints.None)
        {
            return true;
        }
        else if (constraints == Constraints.Count)
        {
            return this.count <= maxCount && this.count >= minCount;
        }
        else if (constraints == Constraints.Quantity)
        {
            float q = float.Parse(this.quantity.ToString("F2"));
            return q - 0.01f <= maxQuantity && q + 0.01f >= minQuantity;
        }

        return true;
    }

    public bool CompareRanges(Ingredient i)
    {
        if(!i.key.Equals(this.key))
        {
            return false;
        }
        if (constraints == Constraints.None)
        {
            return true;
        }
        else if (constraints == Constraints.Count)
        {
            return i.count <= maxCount && i.count >= minCount;
        }
        else if (constraints == Constraints.Quantity)
        {
            float q = float.Parse(i.quantity.ToString("F2"));
            return q - 0.01f <= maxQuantity && q + 0.01f >= minQuantity;
        }

        return true;
    }
}
