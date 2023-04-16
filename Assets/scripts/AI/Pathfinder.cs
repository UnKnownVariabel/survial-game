using System.Collections.Generic;
using UnityEngine;
using System;

// Pathfinder creates paths.
public class Pathfinder
{ 
    // CreatePath creates a path between position and goal.
    public static Path CreatePath(Vector2 position, Vector2 goal, float speed, float health, DestructibleObject target, float dps)
    {
        int dpsMultiplier = (int)(80 / health / speed);
        if (dpsMultiplier > 20)
        {
            dpsMultiplier = 20;
        }
        else if (dpsMultiplier < 1)
        {
            dpsMultiplier = 1;
        }


        Node startNode = GetClosestNode(position);
        Node endNode = GetClosestNode(goal);
        if (startNode == endNode)
        {
            return new Path(new Vector2[] { goal }, new Node[] { startNode  }, target);
        }
        Heap<Node> openSet = new Heap<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            if (currentNode == endNode)
            {
                break;
            }
            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }
                if (openSet.Contains(neighbour))
                {
                    if (neighbour.G > Gcost(neighbour, currentNode))
                    {
                        neighbour.G = Gcost(neighbour, currentNode);
                        neighbour.parent = currentNode;
                        openSet.UpdateItem(neighbour);
                    }
                    continue;
                }
                neighbour.G = Gcost(neighbour, currentNode);
                neighbour.H = Hcost(neighbour);
                neighbour.parent = currentNode;
                openSet.Add(neighbour);
            }
        }
        if (openSet.Count == 0)
        {
            Debug.LogAssertion("failed to find path");
            return null;
        }
        int pathLength = 0;
        int cost = 0;
        for (Node node = endNode; node != startNode; node = node.parent)
        {
            pathLength++;
            cost += node.travelDamage;
        }
        Vector2[] Path = new Vector2[pathLength];
        Node[] nodes = new Node[pathLength];
        pathLength--;
        Path[pathLength] = goal;
        nodes[pathLength] = endNode;
        for (Node node = endNode.parent; node != startNode; node = node.parent)
        {
            pathLength -= 1;
            Path[pathLength] = PathToWorldPos((node.xPos, node.yPos));
            nodes[pathLength] = node;
        }
        return new Path(Path, nodes, target);

        List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                    int checkX = node.xPos + x;
                    int checkY = node.yPos + y;
                    Node new_node = GetNode(checkX, checkY);
                    if (new_node != null)
                    {
                        neighbours.Add(new_node);
                    }
                }
            }
            return neighbours;
        }

        // Returns G cost which is how hard it is to get to specific tile.
        int Gcost(Node node, Node parent)
        {
            int cost = node.travelDamage * dpsMultiplier + node.travelCost + Mathf.Abs((node.xPos - parent.xPos) * (node.yPos - parent.yPos) * node.diagonalExtra) + parent.G;
            if (node.health > 0)
            {
                cost += (int)(2 * node.health / dps);
            }
            return cost;
        }

        // Returns H cost which is airway to the goal
        int Hcost(Node node)
        {
            int dstX = Mathf.Abs(node.xPos - endNode.xPos);
            int dstY = Mathf.Abs(node.xPos - endNode.xPos);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }

    // Returns closest Node to a world position.
    public static Node GetClosestNode(Vector2 position)
    {
        (int x, int y) pathPos = WorldToPathPos(position);
        Node bestNode = null;
        float minDistanceSqr = Mathf.Infinity;
        for(int Yoffset = -1; Yoffset < 3; Yoffset++)
        {
            for (int Xoffset = -1; Xoffset < 3; Xoffset++)
            {
                Node newNode = GetNode(pathPos.x + Xoffset, pathPos.y + Yoffset);
                if (newNode != null)
                {
                    float distanceSqr = (new Vector2(pathPos.x + Xoffset, pathPos.y + Yoffset) - position).sqrMagnitude;
                    if (newNode.health <= 0 && distanceSqr < minDistanceSqr)
                    {
                        minDistanceSqr = distanceSqr;
                        bestNode = newNode;
                    }
                }
            }
        }
        return bestNode;
    }

    // Returns Node based on path cordinats.
    public static Node GetNode(int x, int y)
    {
        const int chunksize = 16;
        int modX = x % chunksize;
        int modY = y % chunksize;
        (int x, int y) key;
        if (modX < 8 && modX > -9)
        {
            key.x = x / chunksize;
        }
        else
        {
            key.x = x / chunksize + Math.Sign(x);
        }
        if (modY < 8 && modY > -9)
        {
            key.y = y / chunksize;
        }
        else
        {
            key.y = y / chunksize + Math.Sign(y);
        }
        Chunk chunk;
        try
        {
            chunk = Globals.chunks[key];
        }
        catch
        {
            return null;
        }
        x = x - key.x * chunksize + chunksize / 2;
        y = y - key.y * chunksize + chunksize / 2;
        if (x > 15 || x < 0)
        {
            Debug.Log("x = " + x.ToString());
        }
        if (y > 15 || y < 0)
        {
            Debug.Log("y = " + y.ToString());
        }
        return chunk.nodes[x, y];
    }

    // Returns path position based on world position.
    public static (int x, int y) WorldToPathPos(Vector3 position)
    {
        return (Mathf.RoundToInt(position.x - 0.5f), Mathf.RoundToInt(position.y - 0.5f));
    }

    // Returns world position based on path position.
    public static Vector3 PathToWorldPos((int x, int y) position)
    {
        return new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
    }
}