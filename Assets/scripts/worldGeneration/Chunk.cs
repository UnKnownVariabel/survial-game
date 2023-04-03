using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
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
    public Dictionary<(int x, int y), Building> buildings = new Dictionary<(int x, int y), Building> ();
    public int minX, minY, maxX, maxY;

    public List<int> itemIndexes { get; private set; }
    public List<Vector2> itemPositions { get; private set; } 

    public Chunk(int X, int Y, float[,] dps, float[,] speed, byte[,] Tiles)
    {
        x = X;
        y = Y;
        DPS = dps;
        this.speed = speed;
        tiles = Tiles;
        itemIndexes = new List<int>();
        itemPositions = new List<Vector2>();
    }

    public Chunk(ChunkData data)
    {
        x = data.x; 
        y = data.y;
        DPS = new float[WorldGeneration.chunkSize, WorldGeneration.chunkSize];
        speed = new float[WorldGeneration.chunkSize, WorldGeneration.chunkSize];
        tiles = new byte[WorldGeneration.chunkSize, WorldGeneration.chunkSize];
        TileData waterData = TileManager.instance.GetData(null);
        TileData sandData = TileManager.instance.GetData(WorldGeneration.instance.sandTile);
        TileData grasData = TileManager.instance.GetData(WorldGeneration.instance.grasTile);
        for(int Y =  0; Y < WorldGeneration.chunkSize; Y++)
        {
            for(int X = 0; X < WorldGeneration.chunkSize; X++)
            {
                tiles[X, Y] = data.tiles[Y * WorldGeneration.chunkSize + X];
                switch (tiles[X, Y] % 16)
                {
                    case 0:
                        DPS[X, Y] = waterData.DPS;
                        speed[X, Y] = waterData.speed;
                        break;
                    case 1:
                        DPS[X, Y] = sandData.DPS;
                        speed[X, Y] = sandData.speed;
                        break;
                    case 2:
                        DPS[X, Y] = grasData.DPS;
                        speed[X, Y] = grasData.speed;
                        break;
                }
            }
        }
        itemIndexes = data.items.ToList();
        itemPositions = data.itemPositions.ToList();
        GenerateNodes();
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

    public void AddItem(Item item)
    {
        items.Add(item);
        itemIndexes.Add(item.data.ItemIndex);
        itemPositions.Add(item.transform.position);
    }

    public void RemoveItem(Item item)
    {
        int i = items.IndexOf(item);
        items.RemoveAt(i);
        itemIndexes.RemoveAt(i);
        itemPositions.RemoveAt(i);
    }
}
