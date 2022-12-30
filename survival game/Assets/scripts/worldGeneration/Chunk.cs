using System.Collections.Generic;
using UnityEngine;


public class Chunk
{
    public bool isSpawnd;
    private float[,] DPS;
    private float[,] Speed;
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
        Speed = speed;
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
                nodes[a, b] = new Node(x * chunkSize + a - chunkSize / 2, y * chunkSize + b - chunkSize / 2, Speed[a, b], DPS[a, b]);
                if (Speed[a, b] == 0)
                {
                    Debug.Log(Speed[a, b]);
                }
            }
        }
    }

    public float getDPS(Vector2 pos)
    {
        Vector2Int position = (Vector2Int)TileManager.instance.tilemap.WorldToCell(pos) + new Vector2Int(DPS.GetLength(0) / 2 - x * WorldGeneration.chunkSize, DPS.GetLength(1) / 2 - y * WorldGeneration.chunkSize);
        return DPS[position.x, position.y];
    }
    public float getSpeed(Vector2 pos)
    {
        Vector2Int position = (Vector2Int)TileManager.instance.tilemap.WorldToCell(pos) + new Vector2Int(Speed.GetLength(0) / 2 - x * WorldGeneration.chunkSize, Speed.GetLength(1) / 2 - y * WorldGeneration.chunkSize);
        return Speed[position.x, position.y];
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
