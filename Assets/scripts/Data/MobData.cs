using System;
using UnityEngine;

// MobData contains all the data needed to save and reconstruct a movingObject.
[Serializable]
public class MobData
{
    public int type;
    public float health;
    public Vector2 position;

    // Constructor converts movingObject to MobData.
    public MobData( MovingObject mob)
    {
        type = mob.mobType;
        health = mob.health;
        position = mob.transform.position;
    }
}
