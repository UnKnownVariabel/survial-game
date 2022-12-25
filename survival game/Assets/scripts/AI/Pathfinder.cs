using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System;

public class Pathfinder
{
    public static int GridSizeX;
    public static int GridSizeY;
    public class Node : IHeapItem<Node>
    {
        public int G;
        public int H;
        //public float speed;
        //public float DPS;
        private float speed;
        private float DPS;
        public int xPos;
        public int yPos;
        public Node parent;
        private int heapIndex;
        public int travelCost;
        public int diagonalExtra;
        public int travelDamage;
        public int F
        {
            get
            {
                return G + H;
            }
        }
        public Node(int x, int y, float Speed, float dps)
        {
            xPos = x;
            yPos = y;
            speed = Speed;
            DPS = dps;
            travelCost = (int)(10 / Speed);
            diagonalExtra = (int)(4 / Speed);
            //diagonalExtra = 4;
            travelDamage = (int)(dps / Speed);
        }
        public void addToSpeed(float value)
        {
            speed *= value;
            travelCost = (int)(10 / speed);
            diagonalExtra = (int)(4 / speed);
            travelDamage = (int)(DPS / speed);
        }
        public void addToDPS(float value)
        {
            DPS += value;
            travelDamage = (int)(DPS / speed);
        }
        public int HeapIndex
        {
            get
            {
                return heapIndex;
            }
            set
            {
                heapIndex = value;
            }
        }
        public int CompareTo(Node nodeToCompare)
        {
            int compare = F.CompareTo(nodeToCompare.F);
            if (compare == 0)
            {
                compare = H.CompareTo(nodeToCompare.H);
            }
            return -compare;
        }
    }
    public static (int x, int y) WorldToPathPos(Vector3 position)
    {
        return (Mathf.RoundToInt(position.x - 0.5f), Mathf.RoundToInt(position.y - 0.5f));
    }
    public static Vector3 PathToWorldPos((int x, int y) position)
    {
        return new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
    }
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
            //Debug.Log("chunk dose not exist: " + key.ToString());
            return null;
        }
        x = x - key.x * chunksize + chunksize / 2;
        y = y - key.y * chunksize + chunksize / 2;
        if(x > 15 || x < 0)
        {
            Debug.Log("x = " + x.ToString());
        }
        if (y > 15 || y < 0)
        {
            Debug.Log("y = " + y.ToString());
        }
        return chunk.nodes[x, y];
    }
    public static Path createPath(Vector2 pos, Vector2 Goal, float speed, float health)
    {
        // checking Getnode against node positions

        int x = 7;
        int y = 14;
        Node test = GetNode(x, y);
        Debug.Log(String.Format("Input: (x: {0}, y: {1}) Node: (x: {2}, y: {3})", x, y, test.xPos, test.yPos));

        //
        System.DateTime startTime = System.DateTime.Now;

        int dpsMultiplier = (int)(80 / health / speed);
        if (dpsMultiplier > 20)
        {
            dpsMultiplier = 20;
        }
        else if (dpsMultiplier < 1)
        {
            dpsMultiplier = 1;
        }
        (int x, int y) position = WorldToPathPos(pos);
        (int x, int y) goal = WorldToPathPos(Goal);

        Node startNode = GetNode(position.x, position.y);
        Node endNode = GetNode(goal.x, goal.y);

        //Debug.Log(startNode.xPos);
        //Debug.Log(endNode.xPos);
        if (startNode == endNode)
        {
            return new Path(new Vector2[] {Goal}, 0);
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
                Debug.Log("current node is endnode");
                break;
            }
            foreach(Node neighbour in getNeighbours(currentNode))
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }
                if (openSet.Contains(neighbour))
                {
                    if(neighbour.G > Gcost(neighbour, currentNode))
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
        int pathLength = 0;
        int cost = 0;
        Debug.Log(closedSet.Count);
        for (Node node = endNode; node != startNode; node = node.parent)
        {
            Debug.Log(node);
            pathLength++;
            cost += node.travelDamage;
        }
        Vector2[] Path = new Vector2[pathLength];
        pathLength--;
        Path[pathLength] = Goal;
        for (Node node = endNode.parent; node != startNode; node = node.parent)
        {
            pathLength -= 1;
            Path[pathLength] = PathToWorldPos((node.xPos, node.yPos));
        }
        Debug.Log("milliseconds to run A*: " + (System.DateTime.Now - startTime).TotalMilliseconds.ToString());
        return new Path(Path, cost*1.4f/speed);
        //return new path(Path);
        List<Node> getNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for(int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    if(x == 0 && y == 0)
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
        int Gcost(Node node, Node parent)
        {
            return node.travelDamage * dpsMultiplier + node.travelCost + Mathf.Abs((node.xPos - parent.xPos)*(node.yPos-parent.yPos)*node.diagonalExtra) + parent.G;
        }
        int Hcost(Node node)
        {
            int dstX = Mathf.Abs(node.xPos - endNode.xPos);
            int dstY = Mathf.Abs(node.xPos - endNode.xPos);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
    /*public static Vector2 getEscapeCord(Vector2 pos, Vector2 Enemy)
    {
        Vector2Int position = (Vector2Int)TileManager.instance.tilemap.WorldToCell(pos) + new Vector2Int(tiles.GetLength(0) / 2, tiles.GetLength(1) / 2);
        //Vector2Int enemy = (Vector2Int)TileManager._instance.tilemap.WorldToCell(Enemy) + new Vector2Int(tiles.GetLength(0) / 2, tiles.GetLength(1) / 2);
        Vector2 Direction = (pos - Enemy).normalized;
        Vector2Int direction = new Vector2Int(Mathf.RoundToInt(Direction.x), Mathf.RoundToInt(Direction.y));
        Vector2Int current = position + direction;
        if(current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
        {
            if (tiles[current.x, current.y].travelDamage == 0)
            {
                return toWorldPos(current);
            }
        }
        if(direction.x + direction.y != 1 && direction.x + direction.y != -1)
        {
            current = position + new Vector2Int(0, direction.y);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
            current = position + new Vector2Int(direction.x, 0);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
            current = position + new Vector2Int(direction.x, -direction.y);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
            current = position + new Vector2Int(-direction.x, direction.y);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }

        }
        else if (direction.y == 0)
        {
            current = position + new Vector2Int(direction.x, -1);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
            current = position + new Vector2Int(direction.x, 1);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
            current = position + new Vector2Int(0, -1);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
            current = position + new Vector2Int(0, 1);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
        }
        else
        {
            current = position + new Vector2Int(1, direction.y);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
            current = position + new Vector2Int(-1, direction.y);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
            current = position + new Vector2Int(-1, 0);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
            current = position + new Vector2Int(1, 0);
            if (current.x < GridSizeX && current.x >= 0 && current.y < GridSizeY && current.y >= 0)
            {
                if (tiles[current.x, current.y].travelDamage == 0)
                {
                    return toWorldPos(current);
                }
            }
        }
        return toWorldPos(position);
        Vector2 toWorldPos(Vector2Int pos)
        {
            return (Vector2)TileManager.instance.tilemap.CellToWorld(new Vector3Int(pos.x - tiles.GetLength(0) / 2, pos.y - tiles.GetLength(1) / 2, 0)) + new Vector2(0.5f, 0.5f);
        }
    }*/
}
