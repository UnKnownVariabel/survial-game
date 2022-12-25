using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public Vector2[] nodes { get; }
    public float cost;
    private int i = 0;
    public Vector2 current
    {
        get
        {
            return nodes[i];
        }
    }
    public Path(Vector2[] Nodes, float Cost)
    {
        nodes = Nodes;
        cost = Cost;
    }
    public bool getNext()
    {
        
        if (i+1 < nodes.Length)
        {
            i++;
            return true;
        }
        return false;
    }
}
