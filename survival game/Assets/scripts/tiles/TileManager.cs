using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    private Dictionary<TileBase, TileData> dataFromTiles;
    [SerializeField] public Tilemap tilemap;
    [SerializeField] private List<TileData> dataFiles;
    [SerializeField] private TileData water;
    public float[,] DPS = new float[20, 20];
    public float[,] Speed = new float[20, 20];
    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();
        foreach(TileData tileData in dataFiles)
        {
            foreach(TileBase tile in tileData.tiles)
            {
                dataFromTiles.Add(tile, tileData);
            }
        }
        if (instance != null)
        {
            Debug.Log("two tile managers have been instantiated");
        }
        instance = this;
    }
    public void Init(float[,] dps, float[,] speed)
    {
        DPS = dps;
        Speed = speed;
    }
    public float getSpeed(Vector3 pos)
    {
        Vector2Int position = (Vector2Int)tilemap.WorldToCell(pos) + new Vector2Int(Speed.GetLength(0) / 2, Speed.GetLength(1) / 2);
        if(position.x >= 0 && position.x < Speed.GetLength(0) && position.y >= 0 && position.y < Speed.GetLength(1))
        {
            return Speed[position.x, position.y];
        }
        else
        {
            return water.speed;
        }
    }
    public float getDPS(Vector3 pos)
    {
        Vector2Int position = (Vector2Int)tilemap.WorldToCell(pos) + new Vector2Int(DPS.GetLength(0) / 2, DPS.GetLength(1) / 2);
        if (position.x >= 0 && position.x < DPS.GetLength(0) && position.y >= 0 && position.y < DPS.GetLength(1))
        {
            return DPS[position.x, position.y];
        }
        else
        {
            return water.DPS;
        }
    }
    /*public void AddDPS(Vector3 pos, float value)
    {
        Vector2Int position = (Vector2Int)tilemap.WorldToCell(pos) + new Vector2Int(DPS.GetLength(0) / 2, DPS.GetLength(1) / 2);
        DPS[position.x, position.y] += value;
        Pathfinder.tiles[position.x, position.y].addToDPS(value);
    }
    public void AddSpeed(Vector3 pos, float value)
    {
        Vector2Int position = (Vector2Int)tilemap.WorldToCell(pos) + new Vector2Int(Speed.GetLength(0) / 2, Speed.GetLength(1) / 2);
        Speed[position.x, position.y] *= value;
        Pathfinder.tiles[position.x, position.y].addToSpeed(value);
    }*/
    public TileBase getTile(Vector2 position)
    {
        return tilemap.GetTile(tilemap.WorldToCell(position));
    }
    public TileData getData(TileBase tile)
    {
        if(tile == null) { return water; }
        return dataFromTiles[tile];
    }
}
