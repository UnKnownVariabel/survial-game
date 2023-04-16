using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Globals contains static variables which need to be accessed globaly and are not singeltons.
public class Globals
{
    public static WorldData worldData;
    public static List<DestructibleObject> destructibleObjects;
    public static List<MovingObject> characters;
    public static List<Mob> mobs;
    public static List<DestructibleObject> targets;
    public static Dictionary<(int, int), Building> walls;
    public static Chunk currentChunk;
    public static Dictionary<(int, int), Chunk> chunks;
    public static Tilemap groundTilemap, decorationTilemap, wallTilemap;

    private static bool _isPaused;
    public static bool pause
    {
        get 
        { 
            return _isPaused;
        }
        set
        {
            _isPaused = value;
            if (value)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    // returns chunk which contains world pos position.
    public static Chunk GetChunk(Vector3 position)
    {
        (int x, int y) key = (Mathf.RoundToInt(position.x / (float)WorldGeneration.chunkSize), Mathf.RoundToInt(position.y / (float)WorldGeneration.chunkSize));
        try
        {
            return chunks[key];
        }
        catch
        {
            return null;
        }
    }

}
