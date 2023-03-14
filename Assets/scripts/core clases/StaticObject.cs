using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObject : DestructibleObject
{
    public byte objectIndex;
    protected override void Start()
    {
        base.Start();
        (int x, int y) pos = chunk.TilePos(transform.position);
        int rest = chunk.tiles[pos.x, pos.y] % 16;
        chunk.tiles[pos.x, pos.y] = (byte)(objectIndex * 16 + rest);
        chunk.staticObjects.Add(pos, this);
    }
    protected override void Die()
    {
        (int x, int y) pos = chunk.TilePos(transform.position);
        int rest = chunk.tiles[pos.x, pos.y] % 16;
        chunk.tiles[pos.x, pos.y] = (byte)rest;
        chunk.staticObjects.Remove(pos);
        base.Die();
    }
}
