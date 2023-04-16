using UnityEngine;
using UnityEngine.Tilemaps;

// WallData contains all the informations and functions needed for a wall item.
[CreateAssetMenu(fileName = "new wall item", menuName = "wall")]
public class WallData : PlacebleItemData
{
    [SerializeField] private TileBase wallTile;

    // is called when wall is placed and adds a wall to the tilemap and as a prefab to the world.
    public override Building placeItem(Vector3 position)
    {
        Building building = base.placeItem(position);
        Vector3Int pos = Globals.wallTilemap.WorldToCell(position);
        Globals.wallTilemap.SetTile(pos, wallTile);
        Globals.GetChunk(position).SettHealth(position, prefab.maxHealth);
        (int, int) key = ((int)position.x, (int)position.y);
        if(!Globals.walls.ContainsKey(key))
        {
            Globals.walls.Add(key, building);
        }
        return building;
    }

    // Is called when wall is to be removed and removes wall from tilemap.
    public override void RemoveItem(Vector2 position)
    {
        Globals.GetChunk(position).SettHealth(position, 0);
        Vector3Int pos = Globals.wallTilemap.WorldToCell(position);
        Globals.wallTilemap.SetTile(pos, null);
        (int, int) key = ((int)position.x, (int)position.y);
        Globals.walls.Remove(key);
    }
}
