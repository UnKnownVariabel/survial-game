using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Globals : MonoBehaviour
{
    public static List<DestructibleObject> destructibleObjects;
    public static List<MovingObject> characters;
    public static List<Mob> mobs;
    public static List<DestructibleObject> targets;
    public static Dictionary<(int, int), Building> walls;
    public static Chunk currentChunk;
    public static Dictionary<(int, int), Chunk> chunks;
    public static Tilemap groundTilemap, decorationTilemap, wallTilemap;
    public static Player player;
    public static TimeHandler timeHandler;
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
