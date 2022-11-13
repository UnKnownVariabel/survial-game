using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static List<DestructibleObject> destructibleObjects;
    public static List<Character> characters;
    public static Chunk currentChunk;
    public static Dictionary<(int, int), Chunk> chunks;
}
