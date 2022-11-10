using System.Collections.Generic;
using UnityEngine;


public class Chunk
{
    public bool isSpawnd;
    private float[,] DPS;
    private float[,] Speed;
    public int x, y;

    public Chunk(int X, int Y, float[,] dps, float[,] speed)
    {
        x = X;
        y = Y;
        DPS = dps;
        Speed = speed;
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
}
