using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new plable item", menuName = "placable item")]
public class PlacebleItemData : ItemData
{
    public Sprite placedSprite;
    public float health;
    public Building prefab;
    public virtual Building placeItem(Vector3 position)
    {
        Vector3 realPos = Globals.wallTilemap.WorldToCell(position) + new Vector3(0.5f, 0.5f);
        Chunk chunk = Globals.GetChunk(position);
        Building building = Instantiate(prefab, realPos, Quaternion.identity);
        building.chunk = chunk;
        return building;
    }
    public override dynamic ReturnFullClass()
    {
        return this;
    }
    public virtual void RemoveItem(Vector2 position)
    {

    }
}
