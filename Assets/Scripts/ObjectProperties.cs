using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectProperties : MonoBehaviour
{
    [SerializeField]
    [Tooltip("A list of key:value information for the object/item")]
    public List<ObjectProperty> properties = new List<ObjectProperty>();

    public void AddOrSet(string key, string value)
    {
        foreach(ObjectProperty prop in properties)
        {
            if(prop.key == key && prop.value != value)
            {
                properties.Remove(prop);
                //prop.SetValue(value);
                break;
                //return;
            }
        }
        properties.Add(new ObjectProperty(key, value));
    }

    public string GetProperty(string key)
    {
        string val = "";
        val = properties.Find( (p) => p.key.Equals(key)).value;
        if (val != null)
            return val;
        else
            return null;
    }

    public bool CompareProperty(string key, string value)
    {
        ObjectProperty p = new ObjectProperty(key, value);
        foreach (ObjectProperty prop in properties)
        {
            if (prop.key == key && !prop.Equals(p))
            {
                return false;
            }
            if(prop.Equals(p))
            {
                return true;
            }
        }
        return false;
    }

    public override string ToString()
    {
        string contents = "";
        foreach (ObjectProperty prop in properties)
        {
            contents += prop.key + ": " + prop.value + ",";
        }
        contents.Remove(contents.Length - 1);
        return contents;
    }
}
