using System.Collections.Generic;
using UnityEngine;


public class Chunk
{
    public bool isSpawnd;
    public float[,] DPS;
    public float[,] speed;
    public byte[,] tiles;
    public Node[,] nodes;
    public int x, y;
    public List<Item> items = new List<Item>();
    //public List<DestructibleObject> trees = new List<DestructibleObject>();
    public Dictionary<(int x, int y), StaticObject> staticObjects = new Dictionary<(int x, int y), StaticObject>();
    public int minX, minY, maxX, maxY;

    public Chunk(int X, int Y, float[,] dps, float[,] speed, byte[,] Tiles)
    {
        x = X;
        y = Y;
        DPS = dps;
        this.speed = speed;
        tiles = Tiles;
    }
    public void GenerateNodes()
    {
        int chunkSize = WorldGeneration.chunkSize;
        nodes = new Node[chunkSize, chunkSize];
        for (int a = 0; a < chunkSize; a++)
        {
            for (int b = 0; b < chunkSize; b++)
            {
                nodes[a, b] = new Node(x * chunkSize + a - chunkSize / 2, y * chunkSize + b - chunkSize / 2, speed[a, b], DPS[a, b]);
                if (speed[a, b] == 0)
                {
                    Debug.Log(speed[a, b]);
                }
            }
        }
    }

    public float GetDPS(Vector2 pos)
    {
        Vector2Int position = (Vector2Int)TileManager.instance.tilemap.WorldToCell(pos) + new Vector2Int(DPS.GetLength(0) / 2 - x * WorldGeneration.chunkSize, DPS.GetLength(1) / 2 - y * WorldGeneration.chunkSize);
        return DPS[position.x, position.y];
    }
    public float GetSpeed(Vector2 pos)
    {
        Vector2Int position = (Vector2Int)TileManager.instance.tilemap.WorldToCell(pos) + new Vector2Int(speed.GetLength(0) / 2 - x * WorldGeneration.chunkSize, speed.GetLength(1) / 2 - y * WorldGeneration.chunkSize);
        return speed[position.x, position.y];
    }
    public (int, int) TilePos(Vector3 position)
    {
        Vector2Int pos = (Vector2Int)TileManager.instance.tilemap.WorldToCell(position) + new Vector2Int(DPS.GetLength(0) / 2 - x * WorldGeneration.chunkSize, DPS.GetLength(1) / 2 - y * WorldGeneration.chunkSize);
        return (pos.x, pos.y);
    }
    public void SettHealth(Vector2 pos, float health)
    {
        (int x, int y) key = TilePos(pos);
        nodes[key.x, key.y].health = health;
    }
}
