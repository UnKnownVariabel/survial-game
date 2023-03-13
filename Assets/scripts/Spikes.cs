using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spikes : Building
{
    public float DPS;
    public float speed;
    private float baseSpeed;
    protected override void Start()
    {
        base.Start();
        (int x, int y) pos = chunk.TilePos(transform.position);
        chunk.DPS[pos.x, pos.y] = DPS;
        baseSpeed = chunk.Speed[pos.x, pos.y];
        chunk.Speed[pos.x, pos.y] = 1 / (1 / baseSpeed + 1 / speed);
    }

    protected override void die()
    {
        (int x, int y) pos = chunk.TilePos(transform.position);
        chunk.DPS[pos.x, pos.y] = 0;
        chunk.Speed[pos.x, pos.y] = baseSpeed;
        base.die();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
