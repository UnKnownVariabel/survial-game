using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Multipliers
{
    public float mob, wood, stone;
    public Multipliers(float Mob, float Wood, float Stone)
    {
        mob = Mob; 
        wood = Wood; 
        stone = Stone;
    }
    public static readonly Multipliers One = new Multipliers(1, 1, 1);
}
