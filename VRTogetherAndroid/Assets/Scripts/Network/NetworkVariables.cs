using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkVariable
{
    public string name;
    public string objID;
}

public class NetworkBool : NetworkVariable
{
    public NetworkBool(string localName, bool val)
    {
        value = val;
        name = localName;
    }

    public bool value;
}

public class NetworkInt : NetworkVariable
{
    public NetworkInt(string localName, int val)
    {
        value = val;
        name = localName;
    }

    public int value;
}

public class NetworkFloat : NetworkVariable
{
    public NetworkFloat(string localName, float val)
    {
        value = val;
        name = localName;
    }

    public float value;
}