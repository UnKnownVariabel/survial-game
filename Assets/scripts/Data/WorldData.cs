using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldData
{
    public static string current_name;
    public string name;
    public Vector2Int offset;
    public ChunkData[] chunks;
    public float time;
    public int day;
    public int[] inventoryTypes;
    public int[] inventoryAmounts;
}
