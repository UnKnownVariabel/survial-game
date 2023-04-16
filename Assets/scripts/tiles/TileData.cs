using UnityEngine;
using UnityEngine.Tilemaps;

// TileData contains data of a tyle type like how fast you walk on the tile.
[CreateAssetMenu(fileName = "new tile data", menuName = "tile date")]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;
    public float DPS, speed, plantSurvivability;
}
