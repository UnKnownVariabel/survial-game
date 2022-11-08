using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class path
{
    private Vector2[] nodes;
    public float cost;
    private int i = 0;
    public Vector2 current
    {
        get
        {
            return nodes[i];
        }
    }
    public path(Vector2[] Nodes, float Cost)
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
