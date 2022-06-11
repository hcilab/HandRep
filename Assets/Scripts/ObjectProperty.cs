using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ObjectProperty
{
    [SerializeField]
    public string key;
    [SerializeField]
    public string value;

    public ObjectProperty(string key, string value)
    {
        this.key = key;
        this.value = value;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        ObjectProperty op = (ObjectProperty)obj;

        return op.key.Equals(this.key) && op.value.Equals(this.value);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return this.key+": " + this.value ;
    }

    public void SetValue(string value)
    {
        this.value = value;
    }
}
