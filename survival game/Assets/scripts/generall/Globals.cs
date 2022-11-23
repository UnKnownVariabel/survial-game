using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Globals : MonoBehaviour
{
    public static List<DestructibleObject> destructibleObjects;
    public static List<Character> characters;
    public static Chunk currentChunk;
    public static Dictionary<(int, int), Chunk> chunks;
    public static Tilemap groundTilemap, decorationTilemap, wallTilemap;
}
