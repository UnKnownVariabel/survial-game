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
    public TileData GetData(TileBase tile)
    {
        if(tile == null) { return water; }
        return dataFromTiles[tile];
    }
}
