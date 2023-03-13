using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new wall item", menuName = "wall")]
public class WallData : PlacebleItemData
{
    [SerializeField] private TileBase WallTile;

    public override Building placeItem(Vector3 position)
    {
        Building building = base.placeItem(position);
        Vector3Int pos = Globals.wallTilemap.WorldToCell(position);
        Globals.wallTilemap.SetTile(pos, WallTile);
        Globals.GetChunk(position).SettHealth(position, health);
        (int, int) key = ((int)position.x, (int)position.y);
        if(!Globals.walls.ContainsKey(key))
        {
            Globals.walls.Add(key, building);
        }
        return building;
    }
    public override void RemoveItem(Vector2 position)
    {
        Globals.GetChunk(position).SettHealth(position, 0);
        Vector3Int pos = Globals.wallTilemap.WorldToCell(position);
        Globals.wallTilemap.SetTile(pos, null);
        (int, int) key = ((int)position.x, (int)position.y);
        Globals.walls.Remove(key);
    }
}
