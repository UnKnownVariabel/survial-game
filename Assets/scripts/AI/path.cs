using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private Vector2[] positions;
    private Node[] nodes;
    private float[] healths;

    // returns lenght of the remaining path;
    public int Length
    {
        get
        {
            return positions.Length - i;
        }
    }
    private int i = 0;
    public Vector2 current
    {
        get
        {
            return positions[i];
        }
    }
    public Path(Vector2[] Positions, Node[] Nodes)
    {
        positions = Positions;
        nodes = Nodes;
        healths = new float[nodes.Length];
        for(int i = 0; i < nodes.Length; i++)
        {
            healths[i] = nodes[i].health;
        }
    }
    public bool getNext()
    {
        // checks if i+1 still is in array and returns false otherwise which indicates the path is complete
        if (i+1 < nodes.Length)
        {
            i++;
            // if a barier has ben placed since the creation of the path this returns false which indicates that the path is complete which forces a new one to be calculated
            return nodes[i].health <= healths[i];
        }
        return false;
    }
}
