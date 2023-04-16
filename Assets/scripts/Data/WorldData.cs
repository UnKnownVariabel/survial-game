using System;
using UnityEngine;

// WorldData contains all the data needed to save and reconstruct a World.
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
    public MobData[] mobs;
    public MobData player;
    public string identifier;

    // Constructor creates unique identifier which is used to identefie the world
    // when submiting a new score to the leaderboard. So the same world dosen't populate
    // the entire leaderboard.
    public WorldData()
    {
        identifier = SystemInfo.deviceUniqueIdentifier.ToString() + UnityEngine.Random.Range(int.MinValue, int.MaxValue).ToString();
    }
}
