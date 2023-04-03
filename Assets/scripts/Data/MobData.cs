using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MobData
{
    public int type;
    public float health;
    public Vector2 position;

    public MobData( MovingObject mob)
    {
        type = mob.mobType;
        health = mob.health;
        position = mob.transform.position;
    }
}
