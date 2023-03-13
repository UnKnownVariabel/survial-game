using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Path
{
    //if there is an obsticle on the path i will be an intermediet target
    public DestructibleObject intermedietTarget;
    public DestructibleObject target;
    // returns lenght of the remaining path;
    public int Length
    {
        get
        {
            return positions.Length - i;
        }
    }
    public bool complete
    {
        get 
        {
            return Length <= 0;
        }
    }
    public Vector2 current
    {
        get
        {
            return positions[i];
        }
    } 
    public DestructibleObject currentTarget
    {
        get
        {
            if (intermedietTarget != null) { return intermedietTarget; }
            return target;
        }
    }

    private Vector2[] positions;
    private Node[] nodes;
    private float[] healths;
    private int nextStop;
    private int i = 0;

    
    public Path(Vector2[] Positions, Node[] Nodes, DestructibleObject target)
    {
        this.target = target;
        positions = Positions;
        nodes = Nodes;
        healths = new float[nodes.Length];
        nextStop = nodes.Length;
        for (int i = 0; i < nodes.Length; i++)
        {
            healths[i] = nodes[i].health;
        }
        // checking for walls in path
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].health > 0)
            {
                (int, int) key = ((int)positions[i].x, (int)positions[i].y);
                if (Globals.walls.ContainsKey(key))
                {
                    intermedietTarget = Globals.walls[key];
                    nextStop = i + 1;
                }
                else
                {
                    Debug.LogError("Globals.walls dose not contain walla att position where health is > 0");
                }
                break;
            }
        }

        this.target = target;
    }
    public bool getNext()
    {
        // checks if i+1 is before stop and returns false otherwise which indicates the path is complete
        if (i+1 < nextStop)
        {
            i++;
            // if a barier has ben placed since the creation of the path this returns false which indicates that the path is complete which forces a new one to be calculated
            return nodes[i].health <= healths[i];
        }
        return false;
    }
    //returns target if there is one left in the path
    public DestructibleObject nextTarget()
    {
        intermedietTarget = null;
        for (int i = nextStop; i < nodes.Length; i++)
        {
            if (nodes[i].health > 0)
            {
                (int, int) key = ((int)positions[i].x, (int)positions[i].y);
                if (Globals.walls.ContainsKey(key))
                {
                    intermedietTarget = Globals.walls[key];
                    nextStop = i + 1;
                }
                else
                {
                    Debug.LogError("Globals.walls dose not contain walla att position where health is > 0");
                }
                break;
            }
        }
        if (intermedietTarget == null)
        {
            nextStop = nodes.Length;
        }
        return currentTarget;        
    }
}
