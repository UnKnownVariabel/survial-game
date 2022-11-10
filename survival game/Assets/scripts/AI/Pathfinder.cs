using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System;

public class Pathfinder
{
    public static node[,] tiles;
    public static int GridSizeX;
    public static int GridSizeY;
    public class node : IHeapItem<node>
    {
        public int G;
        public int H;
        //public float speed;
        //public float DPS;
        private float speed;
        private float DPS;
        public int xPos;
        public int yPos;
        public node parent;
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
        public node(int x, int y, float Speed, float dps)
        {
            xPos = x;
            yPos = y;
            speed = Speed;
            DPS = dps;
            travelCost = (int)(10 / Speed);
            diagonalExtra = (int)(4 / Speed);
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
        public int CompareTo(node nodeToCompare)
        {
            int compare = F.CompareTo(nodeToCompare.F);
            if (compare == 0)
            {
                compare = H.CompareTo(nodeToCompare.H);
            }
            return -compare;
        }
    }
    public static path createPath(Vector2 pos, Vector2 Goal, float speed, float health)
    {
        if(tiles == null) { return null; }
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
        Vector2Int position = (Vector2Int)TileManager.instance.tilemap.WorldToCell(pos) + new Vector2Int(tiles.GetLength(0) / 2, tiles.GetLength(1) / 2);
        Vector2Int goal = (Vector2Int)TileManager.instance.tilemap.WorldToCell(Goal) + new Vector2Int(tiles.GetLength(0) / 2, tiles.GetLength(1) / 2);

        node startNode = tiles[position.x, position.y];
        node endNode = tiles[goal.x, goal.y];
        if(startNode == endNode)
        {
            return new path(new Vector2[] {Goal}, 0);
        }
        Heap<node> openSet = new Heap<node>(tiles.Length);
        HashSet<node> closedSet = new HashSet<node>();
        openSet.Add(startNode);
        while (openSet.Count > 0)
        {
            node currentNode = openSet.RemoveFirst();
            closedSet.Add(currentNode);
            if (currentNode == endNode)
            {
                break;
            }
            foreach(node neighbour in getNeighbours(currentNode))
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
        for (node Node = endNode; Node != startNode; Node = Node.parent)
        {
            pathLength++;
            cost += Node.travelDamage;
        }
        Vector2[] Path = new Vector2[pathLength];
        pathLength--;
        Path[pathLength] = Goal;
        for (node Node = endNode.parent; Node != startNode; Node = Node.parent)
        {
            pathLength -= 1;
            Path[pathLength] = (Vector2)TileManager.instance.tilemap.CellToWorld(new Vector3Int(Node.xPos - tiles.GetLength(0) / 2, Node.yPos - tiles.GetLength(1) / 2, 0)) + new Vector2(0.5f, 0.5f);
        }
        //Debug.Log("milliseconds to run A*: " + (System.DateTime.Now - startTime).TotalMilliseconds.ToString());
        return new path(Path, cost*1.4f/speed);
        //return new path(Path);
        List<node> getNeighbours(node node)
        {
            List<node> neighbours = new List<node>();
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
                    if (checkX >= 0 && checkX < GridSizeX && checkY >= 0 && checkY < GridSizeY)
                    {
                        neighbours.Add(tiles[checkX, checkY]);
                    }
                }
            }
            return neighbours;
        }
        int Gcost(node node, node parent)
        {
            return node.travelDamage * dpsMultiplier + node.travelCost + Mathf.Abs((node.xPos - parent.xPos)*(node.yPos-parent.yPos)*node.diagonalExtra) + parent.G;
        }
        int Hcost(node node)
        {
            int dstX = Mathf.Abs(node.xPos - endNode.xPos);
            int dstY = Mathf.Abs(node.xPos - endNode.xPos);

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY);
            return 14 * dstX + 10 * (dstY - dstX);
        }
    }
    public static Vector2 getEscapeCord(Vector2 pos, Vector2 Enemy)
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
    }
}
