using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : StaticObject
{
    public PlacebleItemData itemData;
    protected override void Die()
    {
        Vector3Int pos = Globals.wallTilemap.WorldToCell(transform.position);
        itemData.RemoveItem(transform.position);
        base.Die();
    }
}
