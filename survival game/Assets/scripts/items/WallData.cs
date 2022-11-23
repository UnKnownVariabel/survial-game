using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "new wall item", menuName = "wall")]
public class WallData : PlacebleItemData
{
    [SerializeField] private TileBase WallTile;

    public override void placeItem(int x, int y)
    {
        Globals.wallTilemap.SetTile(new Vector3Int(x, y, 0), WallTile);
    }
}
