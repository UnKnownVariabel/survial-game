using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : StaticObject
{
    public PlacebleItemData itemData;
    protected override void die()
    {
        Vector3Int pos = Globals.wallTilemap.WorldToCell(transform.position);
        itemData.RemoveItem(pos.x, pos.y);
        base.die();
    }
}
