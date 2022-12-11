using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacebleItemData : ItemData
{
    public Sprite placedSprite;
    public float health;
    public Building prefab;
    public virtual void placeItem(Vector3 position)
    {
        Vector3 realPos = Globals.wallTilemap.WorldToCell(position) + new Vector3(0.5f, 0.5f);
        Chunk chunk = Globals.GetChunk(position);
        Building building = Instantiate(prefab, realPos, Quaternion.identity);
        building.chunk = chunk;
    }
    public override dynamic ReturnFullClass()
    {
        return this;
    }
    public virtual void RemoveItem(int x, int y)
    {

    }
}
