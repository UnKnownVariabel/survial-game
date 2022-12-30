using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new wall item", menuName = "wall")]
public class WallData : PlacebleItemData
{
    [SerializeField] private TileBase WallTile;

    public override void placeItem(Vector3 position)
    {
        base.placeItem(position);
        Vector3Int pos = Globals.wallTilemap.WorldToCell(position);
        Globals.wallTilemap.SetTile(pos, WallTile);
        Globals.GetChunk(position).SettHealth(position, health);
    }
    public override void RemoveItem(Vector2 position)
    {
        Globals.GetChunk(position).SettHealth(position, 0);
        Vector3Int pos = Globals.wallTilemap.WorldToCell(position);
        Globals.wallTilemap.SetTile(pos, null);
    }
}
