using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// TileManager is used to get data from a tile type.
public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    private Dictionary<TileBase, TileData> dataFromTiles;
    [SerializeField] public Tilemap tilemap;
    [SerializeField] private List<TileData> dataFiles;
    [SerializeField] private TileData water;

    // Awake is called when script instance is loaded.
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

    // GetData gets data related to the tile.
    public TileData GetData(TileBase tile)
    {
        if(tile == null) { return water; }
        return dataFromTiles[tile];
    }
}
